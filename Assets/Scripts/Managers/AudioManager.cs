using UnityEngine;
using Assets.Scripts.Settings;

public class AudioManager : MonoBehaviour, IManagers
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;

    public void StartManager()
    {
        GameSettings gameSettings = Managers.settingsManager.gameSettings;
        soundSource.mute = !gameSettings.sound;
        musicSource.mute = !gameSettings.music;
    }

    public void PlaySound(AudioClip clip) => soundSource.PlayOneShot(clip);

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
        Resources.UnloadUnusedAssets();
    }

    public void SetSettings(GameSettings _gameSettigs)
    {
        soundSource.mute = !_gameSettigs.sound;
        musicSource.mute = !_gameSettigs.music;
    }
}
