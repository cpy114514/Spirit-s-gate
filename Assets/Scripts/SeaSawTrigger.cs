using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public PlatformBalance balance;
    public bool isPlatformA;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>() == null) return;

        if (isPlatformA)
            balance.PlayerEnterA();
        else
            balance.PlayerEnterB();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>() == null) return;

        if (isPlatformA)
            balance.PlayerExitA();
        else
            balance.PlayerExitB();
    }
}
