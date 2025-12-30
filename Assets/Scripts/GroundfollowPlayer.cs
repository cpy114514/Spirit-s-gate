using UnityEngine;

public class GroundFollowPlayer : MonoBehaviour
{
    public Transform targetPlayer;   // 要跟随的玩家
    public Vector3 offset;           // 相对偏移（Inspector 调）

    void LateUpdate()
    {
        if (targetPlayer == null) return;

        transform.position = targetPlayer.position + offset;
    }
}
