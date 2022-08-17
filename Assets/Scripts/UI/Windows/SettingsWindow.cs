using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour, IUserInterface
{
    [SerializeField] Toggle sound;
    [SerializeField] Toggle music;
    [SerializeField] Slider quality;
    [SerializeField] Button resetData;
    [SerializeField] AudioClip tapSound;

    public void StartUI()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        sound.isOn = gameSettings.sound;
        music.isOn = gameSettings.music;
        quality.value = (float)gameSettings.quality;
        sound.onValueChanged.AddListener(delegate { SetMuteSettings(); });
        music.onValueChanged.AddListener(delegate { SetMuteSettings(); });
#if UNITY_EDITOR
        resetData.onClick.AddListener(ResetData);
#else
        resetData.gameObject.SetActive(false);
#endif
    }

    void OnDestroy()
    {
        sound.onValueChanged.RemoveAllListeners();
        music.onValueChanged.RemoveAllListeners();
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
        gameSettings.sound = sound.isOn;
        gameSettings.music = music.isOn;
        GameManager.gameManager.SetSettings(gameSettings);
        GameManager.audioManager.PlaySound(tapSound);
    }

    public void SetQualitySettings(float value)
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        gameSettings.quality = (GameSettings.Quality)value;
        GameManager.gameManager.SetSettings(gameSettings);
    }
}
