using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public int maxHealth = 3;

    void Start()
    {
        
        PlayerData.p1Health = maxHealth;
        PlayerData.p2Health = maxHealth;
        

    }
}

