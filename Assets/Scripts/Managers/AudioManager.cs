using UnityEngine;
using Assets.Scripts.Settings;

public class AudioManager : MonoBehaviour, IManagers
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;

    public void StartManager()
    {
        SetSourcesSettings();
    }

    public void PlaySound(AudioClip clip)
    {
        SetSourcesSettings();
        soundSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        SetSourcesSettings();
        musicSource.clip = clip;
        musicSource.Play();
        Resources.UnloadUnusedAssets();
    }

    void SetSourcesSettings()
    {
        GameSettings gameSettings = Managers.settingsManager.gameSettings;
        soundSource.mute = !gameSettings.sound;
        musicSource.mute = !gameSettings.music;
    }
}
