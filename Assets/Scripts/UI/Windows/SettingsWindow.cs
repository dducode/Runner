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
    }

    public void SetMusicSettings(bool value)
    {
        GameSettings gameSettings = Managers.settingsManager.gameSettings;
        gameSettings.music = value;
        Managers.settingsManager.SetSettings(gameSettings);
        Managers.audioManager.PlaySound(tapSound);
    }

    public void SetSoundSettings(bool value)
    {
        GameSettings gameSettings = Managers.settingsManager.gameSettings;
        gameSettings.sound = value;
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
