using UnityEngine;
using UnityEngine.UI;
using Settings;

public class SettingsWindow : MonoBehaviour, IUserInterface
{
    [SerializeField] Toggle sound;
    [SerializeField] Toggle music;
    [SerializeField] Slider quality;
    [SerializeField] AudioClip tapSound;

    public void StartUI()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
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
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        gameSettings.sound = sound.isOn;
        gameSettings.music = music.isOn;
        GameManager.gameManager.SetSettings(gameSettings);
        GameManager.audioManager.PlaySound(tapSound);
    }

    public void SetQualitySettings(float value)
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        gameSettings.quality = (Quality)value;
        GameManager.gameManager.SetSettings(gameSettings);
    }
}
