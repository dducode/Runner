using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] Toggle soundMute;
    [SerializeField] AudioClip tapSound;

    void Awake()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        soundMute.isOn = gameSettings.soundMute;
    }

    void OnEnable()
    {
        soundMute.onValueChanged.AddListener(delegate { SetGameSettings(); });
    }
    void OnDisable()
    {
        soundMute.onValueChanged.RemoveAllListeners();
    }

    void SetGameSettings()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        gameSettings.soundMute = soundMute.isOn;
        GameManager.gameManager.SetGameSettings(gameSettings);
        GameManager.audioManager.PlaySound(tapSound);
    }

    public void ResetData()
    {
        GameManager.audioManager.PlaySound(tapSound);
        GameManager.dataManager.ResetData();
    }
}
