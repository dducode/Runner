using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Settings;

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
    float[][] screenResolution;

    AsyncOperation async;
    Scene scene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameSettings();
        screenResolution = new float[][] {
            new float[] { Screen.currentResolution.width * 0.6f, Screen.currentResolution.height * 0.6f },
            new float[] { Screen.currentResolution.width * 0.8f, Screen.currentResolution.height * 0.8f },
            new float[] { Screen.currentResolution.width * 1f, Screen.currentResolution.height * 1f },
        };
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
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        QualitySettings.SetQualityLevel((int)gameSettings.quality);
        Screen.SetResolution(
            (int)screenResolution[(int)gameSettings.quality][0],
            (int)screenResolution[(int)gameSettings.quality][1], true);
        LoadScene(1);
    }

    void OnEnable() => BroadcastMessages<bool>.AddListener(Messages.PAUSE, IsPause);
    void OnDisable() => BroadcastMessages<bool>.RemoveListener(Messages.PAUSE, IsPause);

    public void SetSettings(GameSettings _gameSettings)
    {
        gameSettings = _gameSettings;
        QualitySettings.SetQualityLevel((int)gameSettings.quality);
        int width = (int)screenResolution[(int)gameSettings.quality][0];
        int height = (int)screenResolution[(int)gameSettings.quality][1];
        Screen.SetResolution(
            (int)screenResolution[(int)gameSettings.quality][0],
            (int)screenResolution[(int)gameSettings.quality][1], true);
        audioManager.SetSettings(gameSettings);
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
        uiManager.UpdateViews();
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
        int savedSound = Convert.ToInt32(gameSettings.sound);
        int savedMusic = Convert.ToInt32(gameSettings.music);
        int savedQuality = (int)gameSettings.quality;

        PlayerPrefs.SetInt("Sound", savedSound);
        PlayerPrefs.SetInt("Music", savedMusic);
        PlayerPrefs.SetInt("Quality", savedQuality);
        PlayerPrefs.Save();
    }
    void LoadGameSettings()
    {
        //загружаем настройки пользователя
        int savedSound, savedMusic;
        Quality quality;
        if (PlayerPrefs.HasKey("Sound") && PlayerPrefs.HasKey("Music"))
        {
            savedSound = PlayerPrefs.GetInt("Sound");
            savedMusic = PlayerPrefs.GetInt("Music");
            quality = (Quality)PlayerPrefs.GetInt("Quality");
            GameSettings _gameSettings;
            _gameSettings.sound = Convert.ToBoolean(savedSound);
            _gameSettings.music = Convert.ToBoolean(savedMusic);
            _gameSettings.quality = quality;
            gameSettings = _gameSettings;
        }
        else
            gameSettings = defaultSettings;
    }
}
