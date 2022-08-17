using UnityEngine;

public class AudioManager : MonoBehaviour, IManagers
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] string startMusic;

    public void StartManager()
    {
        GameSettings gameSettings = GameManager.gameManager.gameSettings;
        soundSource.mute = !gameSettings.sound;
        musicSource.mute = !gameSettings.music;
        PlayMusic(Resources.Load("Musics/" + startMusic) as AudioClip);
    }

    public void PlaySound(AudioClip clip) => soundSource.PlayOneShot(clip);

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void SetSettings(GameSettings _gameSettigs)
    {
        soundSource.mute = !_gameSettigs.sound;
        musicSource.mute = !_gameSettigs.music;
    }
}
