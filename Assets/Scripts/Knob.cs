using UnityEngine;
using UnityEngine.Events;

public class KnobController2D : MonoBehaviour
{
    [Header("基础设置")]
    public Camera mainCamera;
    public float rotationSpeed = 8f;     // 旋钮自身平滑速度

    [Header("旋钮角度限制")]
    public bool useAngleLimit = true;
    public float minAngle = -90f;
    public float maxAngle = 90f;

    [Header("吸附设置")]
    public bool useSnap = true;
    [Range(1f, 90f)] public float snapStep = 15f;

    [Header("控制对象列表")]
    public ControlledTarget[] targets; // ✅ 多对象控制数组

    [Header("触发事件（可选）")]
    public bool enableTrigger = false;
    public float triggerAngle = 45f;
    public UnityEvent onTriggered;

    public enum ControlMode { Rotate, Move }
    public enum MoveAxis { Horizontal, Vertical }

    [System.Serializable]
    public class ControlledTarget
    {
        [Tooltip("要控制的对象")]
        public Transform controlledObject;

        [Tooltip("控制模式：旋转或移动")]
        public ControlMode controlMode = ControlMode.Rotate;

        [Tooltip("旋钮转动 ±degreesForFullTravel 度时移动达到最大距离")]
        public float degreesForFullTravel = 90f;

        [Tooltip("最大移动距离（移动模式）或旋转角度（旋转模式）")]
        public float moveDistance = 3f;

        [Tooltip("移动方向（移动模式）")]
        public MoveAxis moveAxis = MoveAxis.Horizontal;

        [Tooltip("是否反向")]
        public bool reverseDirection = false;

        [Tooltip("移动速度倍率（越小越慢）")]
        public float moveSpeed = 1f; // ✅ 新增速度倍率

        [HideInInspector] public Vector3 initialPosition;
    }

    // 运行时
    private bool isDragging = false;
    private float targetAngle = 0f;
    private float startOffset = 0f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
                Debug.LogWarning("KnobController2D: 没有找到主相机，请在 Inspector 中手动指定！");
        }

        foreach (var t in targets)
        {
            if (t.controlledObject != null)
                t.initialPosition = t.controlledObject.position;

            if (t.degreesForFullTravel <= 0f)
                t.degreesForFullTravel = 1f;
        }
    }

    void Update()
    {
        // 1️⃣ 旋钮自身平滑旋转
        float currentAngle = transform.eulerAngles.z;
        if (currentAngle > 180f) currentAngle -= 360f;
        float smoothedAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);

        // 2️⃣ 控制每个对象
        foreach (var t in targets)
        {
            if (t.controlledObject == null) continue;

            float effectiveAngle = t.reverseDirection ? -smoothedAngle : smoothedAngle;

            if (t.controlMode == ControlMode.Rotate)
            {
                // ✅ 旋转也受 moveSpeed 影响
                float rotationValue = effectiveAngle * t.moveSpeed;
                t.controlledObject.rotation = Quaternion.Euler(0, 0, rotationValue);
            }
            else // ControlMode.Move
            {
                // ✅ 新逻辑：用 PingPong 让移动往返，并带速度倍率
                float angleForSpeed = effectiveAngle * t.moveSpeed;
                float cycle = Mathf.PingPong(angleForSpeed / t.degreesForFullTravel, 2f) - 1f;
                float ratioSigned = Mathf.Clamp(cycle, -1f, 1f);

                // 计算位移
                float displacement = ratioSigned * t.moveDistance;

                Vector3 offset = (t.moveAxis == MoveAxis.Horizontal)
                    ? new Vector3(displacement, 0f, 0f)
                    : new Vector3(0f, displacement, 0f);

                t.controlledObject.position = t.initialPosition + offset;
            }
        }

        // 3️⃣ 触发事件
        if (enableTrigger && Mathf.Abs(targetAngle - triggerAngle) < 2f)
            onTriggered?.Invoke();
    }

    void OnMouseDown()
    {
        isDragging = true;
        if (mainCamera == null) return;

        Vector3 dir = Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);
        float startAngleScreen = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        startOffset = startAngleScreen - targetAngle;
    }

    void OnMouseDrag()
    {
        if (!isDragging || mainCamera == null) return;

        Vector3 dir = Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);
        float angleScreen = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float newAngle = angleScreen - startOffset;

        if (useAngleLimit)
            newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);

        targetAngle = newAngle;
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (useSnap)
            targetAngle = Mathf.Round(targetAngle / snapStep) * snapStep;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position;
        float radius = 1f;
        Vector3 startDir = Quaternion.Euler(0, 0, minAngle) * Vector3.right;
        Vector3 endDir = Quaternion.Euler(0, 0, maxAngle) * Vector3.right;
        Gizmos.DrawLine(center, center + startDir * radius);
        Gizmos.DrawLine(center, center + endDir * radius);
    }
}
