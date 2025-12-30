using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // ✅ 从父物体查找 PlayerHealth（适配子 Collider）
        PlayerHealth health = col.GetComponentInParent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
