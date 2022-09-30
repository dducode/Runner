using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Settings;

public class SettingsWindow : MonoBehaviour, IUserInterface
{
    [SerializeField] Toggle sound;
    [SerializeField] Toggle music;
    [SerializeField] Slider quality;
    [SerializeField] AudioClip tapSound;

    public void StartUI()
    {
        GameSettings gameSettings = Managers.settingsManager.gameSettings;
        sound.isOn = gameSettings.sound;
        music.isOn = gameSettings.music;
        quality.value = (float)gameSettings.quality;
        sound.onValueChanged.AddListener(delegate { SetMuteSettings(); });
        music.onValueChanged.AddListener(delegate { SetMuteSettings(); });
    }

    void OnDestroy()
    {
        sound.onValueChanged.RemoveAllListeners();
        music.onValueChanged.RemoveAllListeners();
    }

    void SetMuteSettings()
    {
        GameSettings gameSettings = Managers.settingsManager.gameSettings;
        gameSettings.sound = sound.isOn;
        gameSettings.music = music.isOn;
        Managers.settingsManager.SetSettings(gameSettings);
        Managers.audioManager.PlaySound(tapSound);
    }

    public void SetQualitySettings(float value)
    {
        GameSettings gameSettings = Managers.settingsManager.gameSettings;
        gameSettings.quality = (Quality)value;
        Managers.settingsManager.SetSettings(gameSettings);
    }
}
