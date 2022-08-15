using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour, IUserInterface
{
    [SerializeField] Toggle soundMute;
    [SerializeField] Toggle musicMute;
    [SerializeField] Slider quality;
    [SerializeField] Button resetSettings;
    [SerializeField] Button resetData;
    [SerializeField] AudioClip tapSound;

    public void StartUI()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        soundMute.isOn = gameSettings.soundMute;
        musicMute.isOn = gameSettings.musicMute;
        quality.value = (float)gameSettings.quality;
        soundMute.onValueChanged.AddListener(delegate { SetMuteSettings(); });
        musicMute.onValueChanged.AddListener(delegate { SetMuteSettings(); });
        resetSettings.interactable = !gameSettings.Equals(GameManager.gameManager.DefaultSettings);
#if UNITY_EDITOR
        resetData.onClick.AddListener(ResetData);
#else
        resetData.gameObject.SetActive(false);
#endif
    }

    void OnDestroy()
    {
        soundMute.onValueChanged.RemoveAllListeners();
        musicMute.onValueChanged.RemoveAllListeners();
#if UNITY_EDITOR
        resetData.onClick.RemoveAllListeners();
#endif
    }

#if UNITY_EDITOR
    public void ResetData()
    {
        GameManager.audioManager.PlaySound(tapSound);
        GameManager.dataManager.ResetData();
    }
#endif

    void SetMuteSettings()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        gameSettings.soundMute = soundMute.isOn;
        gameSettings.musicMute = musicMute.isOn;
        GameManager.gameManager.SetGameSettings(gameSettings);
        GameManager.audioManager.PlaySound(tapSound);
        resetSettings.interactable = !gameSettings.Equals(GameManager.gameManager.DefaultSettings);
    }

    public void SetQualitySettings(float value)
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        gameSettings.quality = (GameSettings.Quality)value;
        GameManager.gameManager.SetGameSettings(gameSettings);
        resetSettings.interactable = !gameSettings.Equals(GameManager.gameManager.DefaultSettings);
    }

    public void ResetSettings()
    {
        GameManager.audioManager.PlaySound(tapSound);
        GameManager.gameManager.ResetGameSettings();
        soundMute.onValueChanged.RemoveAllListeners();
        musicMute.onValueChanged.RemoveAllListeners();
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        soundMute.isOn = gameSettings.soundMute;
        musicMute.isOn = gameSettings.musicMute;
        quality.value = (float)gameSettings.quality;
        soundMute.onValueChanged.AddListener(delegate { SetMuteSettings(); });
        musicMute.onValueChanged.AddListener(delegate { SetMuteSettings(); GameManager.audioManager.PlayMusic(); });
        resetSettings.interactable = false;
    }
}
