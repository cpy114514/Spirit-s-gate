using UnityEngine;

public class PlatformBalance : MonoBehaviour
{
    [Header("Platforms")]
    public Transform platformA;
    public Transform platformB;

    [Header("Settings")]
    public float maxOffset = 1.5f;     // 最大上下位移
    public float moveSpeed = 3f;        // 移动速度

    private Vector3 aStartPos;
    private Vector3 bStartPos;

    private int aCount = 0;
    private int bCount = 0;

    void Start()
    {
        aStartPos = platformA.position;
        bStartPos = platformB.position;
    }

    void Update()
    {
        // 计算目标偏移
        float target = 0f;

        if (aCount > bCount)
            target = -maxOffset;
        else if (bCount > aCount)
            target = maxOffset;

        // A 和 B 反向移动
        Vector3 aTarget = aStartPos + Vector3.up * target;
        Vector3 bTarget = bStartPos + Vector3.up * -target;

        platformA.position = Vector3.Lerp(platformA.position, aTarget, Time.deltaTime * moveSpeed);
        platformB.position = Vector3.Lerp(platformB.position, bTarget, Time.deltaTime * moveSpeed);
    }

    // 由平台调用
    public void PlayerEnterA() => aCount++;
    public void PlayerExitA() => aCount--;

    public void PlayerEnterB() => bCount++;
    public void PlayerExitB() => bCount--;
}
