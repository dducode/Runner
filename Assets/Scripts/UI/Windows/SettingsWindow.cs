using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] Toggle soundMute;
    [SerializeField] Toggle musicMute;
    [SerializeField] AudioClip tapSound;

    public void StartUI()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        soundMute.isOn = gameSettings.soundMute;
        musicMute.isOn = gameSettings.musicMute;
        soundMute.onValueChanged.AddListener(delegate { SetGameSettings(); });
        musicMute.onValueChanged.AddListener(delegate { SetGameSettings(); GameManager.audioManager.PlayMusic(); });
    }

    void OnDestroy()
    {
        soundMute.onValueChanged.RemoveAllListeners();
        musicMute.onValueChanged.RemoveAllListeners();
    }

    void SetGameSettings()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        gameSettings.soundMute = soundMute.isOn;
        gameSettings.musicMute = musicMute.isOn;
        GameManager.gameManager.SetGameSettings(gameSettings);
        GameManager.audioManager.PlaySound(tapSound);
    }

    public void ResetData()
    {
        GameManager.audioManager.PlaySound(tapSound);
        GameManager.dataManager.ResetData();
    }
}
