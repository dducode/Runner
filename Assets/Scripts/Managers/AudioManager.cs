using UnityEngine;

public class AudioManager : MonoBehaviour, IManagers
{
    GameSettings gameSettings;
    AudioSource soundSource;
    AudioSource musicSource;
    [SerializeField] string startMusic;

    public void StartManager()
    {
        DontDestroyOnLoad(gameObject);
        soundSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        gameSettings = GameManager.gameManager.gameSettings;
        PlayMusic(Resources.Load("Musics/" + startMusic) as AudioClip);
    }

    public void PlaySound(AudioClip clip)
    {
        gameSettings = GameManager.gameManager.gameSettings;
        if (!gameSettings.soundMute)
            soundSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip = null)
    {
        if (clip != null)
            musicSource.clip = clip;
        gameSettings = GameManager.gameManager.gameSettings;
        if (!gameSettings.musicMute)
            musicSource.Play();
        else
            musicSource.Stop();
    }
}
