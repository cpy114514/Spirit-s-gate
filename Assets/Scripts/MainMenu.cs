using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;


     [Header("Scene Name To Load")]
    public string gameSceneName = "Level1";

    public void StartGameNow()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void Start()
    {
        // ȷ����ʼ��ʾ���˵�
        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SaveSlots"); // ���������Ϸ������
    }

    public void OpenSettings()
    {
        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
