using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    [System.Serializable]
    public class KeyBinding
    {
        public string actionName;
        public Button button;
        public KeyCode defaultKey;
    }

    [System.Serializable]
    public class PlayerKeyBindings
    {
        public string playerName;
        public List<KeyBinding> keyBindings = new List<KeyBinding>();
    }

    public List<PlayerKeyBindings> players = new List<PlayerKeyBindings>();
    public Text infoText;

    private string waitingForPlayer = null;
    private string waitingForAction = null;

    void Start()
    {
        SetupButtons();
        if (infoText != null)
            infoText.text = "";
    }

    // 自动绑定按钮事件
    void SetupButtons()
    {
        foreach (var player in players)
        {
            foreach (var binding in player.keyBindings)
            {
                // 初始化按钮文字
                KeyCode key = GetSavedKey(player.playerName, binding.actionName, binding.defaultKey);
                UpdateButtonText(player.playerName, binding.actionName, key);

                // 清除旧监听，添加新监听
                binding.button.onClick.RemoveAllListeners();
                string pName = player.playerName;
                string action = binding.actionName;
                binding.button.onClick.AddListener(() => OnClickChangeKey(pName, action));
            }
        }
    }

    public void OnClickChangeKey(string playerName, string actionName)
    {
        waitingForPlayer = playerName;
        waitingForAction = actionName;
        if (infoText != null)
            infoText.text = "Press a key...";
    }

    void Update()
    {
        if (waitingForPlayer != null && waitingForAction != null)
        {
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    SaveKey(waitingForPlayer, waitingForAction, kcode);
                    UpdateButtonText(waitingForPlayer, waitingForAction, kcode);
                    waitingForPlayer = null;
                    waitingForAction = null;
                    if (infoText != null)
                        infoText.text = "";
                    break;
                }
            }
        }
    }

    void SaveKey(string playerName, string actionName, KeyCode key)
    {
        PlayerPrefs.SetString($"{playerName}_{actionName}", key.ToString());
        PlayerPrefs.Save();
    }

    KeyCode GetSavedKey(string playerName, string actionName, KeyCode defaultKey)
    {
        string savedKey = PlayerPrefs.GetString($"{playerName}_{actionName}", defaultKey.ToString());
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), savedKey);
    }

    void UpdateButtonText(string playerName, string actionName, KeyCode key)
    {
        foreach (var player in players)
        {
            if (player.playerName == playerName)
            {
                foreach (var binding in player.keyBindings)
                {
                    if (binding.actionName == actionName)
                    {
                        Text btnText = binding.button.GetComponentInChildren<Text>();
                        if (btnText != null)
                            btnText.text = $"{playerName} {actionName}: {key}";
                        return;
                    }
                }
            }
        }
    }

    // PlayerController 调用获取当前键位
    public static KeyCode GetKey(string playerName, string actionName, KeyCode defaultKey)
    {
        string savedKey = PlayerPrefs.GetString($"{playerName}_{actionName}", defaultKey.ToString());
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), savedKey);
    }
}
