using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn Point")]
    public Transform spawnPoint;   // 出生点引用

    private PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        // 开局时把玩家放到出生点
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }

    // 外部调用：重生
    public void Respawn()
    {
        if (spawnPoint != null)
        {
            // 停止全部速度
            controller.RB.velocity = Vector2.zero;

            // 移动到出生点
            transform.position = spawnPoint.position;
        }
    }
}
    