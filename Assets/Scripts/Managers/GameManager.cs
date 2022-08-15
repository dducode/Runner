using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }
    public static AudioManager audioManager { get; private set; }
    public static DataManager dataManager { get; private set; }
    public static UIManager uiManager { get; private set; }
    List<IManagers> managers;

    [SerializeField] GameSettings defaultSettings;
    public GameSettings gameSettings { get; private set; }
    public GameSettings DefaultSettings { get { return defaultSettings; } }   

    AsyncOperation async;
    Scene scene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameSettings();
        gameManager = this;
        audioManager = GetComponentInChildren<AudioManager>();
        dataManager = GetComponentInChildren<DataManager>();
        uiManager = GetComponentInChildren<UIManager>();
        managers = new List<IManagers>();
        managers.Add(audioManager);
        managers.Add(dataManager);
        managers.Add(uiManager);
        foreach (IManagers manager in managers)
            manager.StartManager();
        Application.quitting += SaveGameSettings;
        Application.quitting += dataManager.SaveGameData;
        Application.targetFrameRate = 60;
        LoadScene(1);
    }

    void OnEnable() => BroadcastMessages<bool>.AddListener(Messages.PAUSE, IsPause);
    void OnDisable() => BroadcastMessages<bool>.RemoveListener(Messages.PAUSE, IsPause);

    public void SetGameSettings(GameSettings _gameSettings)
    {
        gameSettings = _gameSettings;
        QualitySettings.SetQualityLevel((int)gameSettings.quality);
        audioManager.PlayMusic();
    }
    public void ResetGameSettings()
    {
        gameSettings = defaultSettings;
        QualitySettings.SetQualityLevel((int)gameSettings.quality);
        audioManager.PlayMusic();
    }

    public void LoadScene(int scene)
    {
        async = SceneManager.LoadSceneAsync(scene);
        StartCoroutine(LoadProgress());
        async.completed += LoadCompleted;
    }
    void LoadCompleted(AsyncOperation _async)
    {
        BroadcastMessages<bool>.SendMessage(Messages.PAUSE, false);
        scene = SceneManager.GetActiveScene();
        uiManager.ActiveUI(scene.buildIndex);
        async.completed -= LoadCompleted;
    }
    IEnumerator LoadProgress()
    {
        while (!async.isDone)
        {
            uiManager.LoadScene(async.progress);
            yield return null;
        }
    }
    public void IsPause(bool isPause)
    {
        Time.timeScale = isPause ? 0f : 1f;
        dataManager.SaveGameData();
        SaveGameSettings();
    }
    public void OnApplicationPause(bool pause) => BroadcastMessages<bool>.SendMessage(Messages.PAUSE, pause);

    void SaveGameSettings()
    {
        // сохраняем настройки пользователя
        int savedSoundMute = gameSettings.soundMute ? 1 : 0;
        int savedMusicMute = gameSettings.musicMute ? 1 : 0;
        int savedQuality = (int)gameSettings.quality;

        PlayerPrefs.SetInt("SoundMute", savedSoundMute);
        PlayerPrefs.SetInt("MusicMute", savedMusicMute);
        PlayerPrefs.SetInt("Quality", savedQuality);
        PlayerPrefs.Save();
    }
    void LoadGameSettings()
    {
        //загружаем настройки пользователя
        int savedSoundMute, savedMusicMute;
        GameSettings.Quality quality;
        if (PlayerPrefs.HasKey("SoundMute") && PlayerPrefs.HasKey("MusicMute"))
        {
            savedSoundMute = PlayerPrefs.GetInt("SoundMute");
            savedMusicMute = PlayerPrefs.GetInt("MusicMute");
            quality = (GameSettings.Quality)PlayerPrefs.GetInt("Quality");
            GameSettings _gameSettings;
            _gameSettings.soundMute = savedSoundMute is 1;
            _gameSettings.musicMute = savedMusicMute is 1;
            _gameSettings.quality = quality;
            gameSettings = _gameSettings;
        }
        else
            gameSettings = defaultSettings;
    }
}
