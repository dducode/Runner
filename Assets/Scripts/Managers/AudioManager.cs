using UnityEngine;

public class AudioManager : MonoBehaviour, IManagers
{
    GameSettings gameSettings;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] string startMusic;

    public void StartManager()
    {
        gameSettings = GameManager.gameManager.gameSettings;
        PlayMusic(Resources.Load("Musics/" + startMusic) as AudioClip);
    }

    public void PlaySound(AudioClip clip)
    {
        gameSettings = GameManager.gameManager.gameSettings;
        if (!gameSettings.soundMute)
            soundSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip = default)
    {
        if (clip is not default(AudioClip))
            musicSource.clip = clip;
        gameSettings = GameManager.gameManager.gameSettings;
        if (!gameSettings.musicMute)
            musicSource.Play();
        else
            musicSource.Stop();
    }
}
