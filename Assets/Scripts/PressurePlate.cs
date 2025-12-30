using UnityEngine;



public class PressurePlate : MonoBehaviour
{
    [Header("Plate State")]
    public bool isPressed = false;

    private int objectsOnPlate = 0;

    public DoorSimple door;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            objectsOnPlate++;
            UpdatePlateState();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            objectsOnPlate--;
            UpdatePlateState();
        }
    }

    void UpdatePlateState()
    {
        bool newState = objectsOnPlate > 0;

        if (newState != isPressed)
        {
            isPressed = newState;

            if (isPressed)
                OnPressed();
            else
                OnReleased();
        }
    }

    void OnPressed()
    {
        Debug.Log("🟢 Plate Pressed");
        if (door != null)
            door.Open();
    }

    void OnReleased()
    {
        Debug.Log("🔴 Plate Released");
        if (door != null)
            door.Close();
    }

}
