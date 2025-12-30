using UnityEngine;

public class SingleDoor : MonoBehaviour
{
    [Header("Which Player?")]
    public PlayerController targetPlayer;

    [Header("Enter Key (Set in Inspector)")]
    public KeyCode enterKey = KeyCode.E;

    [HideInInspector] public bool playerInside = false;
    [HideInInspector] public bool playerEntered = false;

    private Vector3 savedPos;
    private SpriteRenderer sr;

    void Start()
    {
        sr = targetPlayer.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController p = col.GetComponent<PlayerController>();
        if (p == targetPlayer)
        {
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        PlayerController p = col.GetComponent<PlayerController>();
        if (p == targetPlayer)
        {
            playerInside = false;
            // ❌ 不要在这里强制退出门！
        }
    }

    void Update()
    {
        // 只有在触发区内，才能按键
        if (!playerInside) return;

        if (Input.GetKeyDown(enterKey))
        {
            if (!playerEntered)
                EnterDoor();
            else
                ExitDoor();
        }
    }

    void EnterDoor()
    {
        playerEntered = true;

        savedPos = targetPlayer.transform.position;

        targetPlayer.isFrozen = true;
        targetPlayer.RB.velocity = Vector2.zero;

        if (sr != null)
            sr.enabled = false;
    }

    void ExitDoor()
    {
        playerEntered = false;

        targetPlayer.transform.position = savedPos;
        targetPlayer.isFrozen = false;

        if (sr != null)
            sr.enabled = true;
    }

    // 供 DoorManager / 关卡管理器调用
    public void ForceExit()
    {
        if (!playerEntered) return;
        ExitDoor();
    }
}
