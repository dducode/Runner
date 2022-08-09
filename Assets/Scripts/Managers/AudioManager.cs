using UnityEngine;

public class AudioManager : MonoBehaviour, IManagers
{
    GameSettings gameSettings;
    AudioSource soundSource;

    public void StartManager()
    {
        DontDestroyOnLoad(gameObject);
        soundSource = gameObject.AddComponent<AudioSource>();
        gameSettings = GameManager.gameManager.gameSettings;
    }

    public void PlaySound(AudioClip clip)
    {
        gameSettings = GameManager.gameManager.gameSettings;
        if(!gameSettings.soundMute)
            soundSource.PlayOneShot(clip);
    }
}
