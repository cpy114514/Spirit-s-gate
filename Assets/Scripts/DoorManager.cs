using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    [Header("Two Doors")]
    public SingleDoor door1;     // Player1 的门
    public SingleDoor door2;     // Player2 的门

    [Header("Next Scene")]
    public string nextSceneName;

    void Update()
    {
        if (door1.playerEntered && door2.playerEntered)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
