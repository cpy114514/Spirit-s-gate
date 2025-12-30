using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player")]
    public string playerID = "P1";   // 只能是 "P1" 或 "P2"

    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Respawn")]
    public Transform spawnPoint;
    public string respawnScene;

    [Header("UI")]
    public HeartUI heartUI;

    void Start()
    {
        if (playerID == "P1")
        {
            currentHealth = PlayerData.p1Health;
        }
        else if (playerID == "P2")
        {
            currentHealth = PlayerData.p2Health;
        }
        else
        {
            Debug.LogError($"❌ Unknown playerID: {playerID}");
            currentHealth = maxHealth;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (spawnPoint != null)
            transform.position = spawnPoint.position;
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SaveHealth();
        UpdateUI();

        // ⭐ 死亡 → 进 Respawn Scene
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(respawnScene);
            return;
        }

        // ⭐ 普通受伤 → 回出生点
        if (spawnPoint != null)
            transform.position = spawnPoint.position;
    }

    void SaveHealth()
    {
        // ⭐ 只有这里能写 PlayerData
        if (playerID == "P1")
            PlayerData.p1Health = currentHealth;
        else
            PlayerData.p2Health = currentHealth;

        Debug.Log($"[SAVE] P1={PlayerData.p1Health} P2={PlayerData.p2Health}");
    }

    void UpdateUI()
    {
        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth, maxHealth);
    }
}
