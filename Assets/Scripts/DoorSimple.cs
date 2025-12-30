using UnityEngine;

public class DoorSimple : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(false); // √≈œ˚ ß
    }

    public void Close()
    {
        gameObject.SetActive(true);
    }
}
