using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Settings;
using System;

///<summary>
///Класс для доступа к игровым настройкам
///</summary>
public class GameSettingsManager : MonoBehaviour, IManagers
{
    [SerializeField] GameSettings defaultSettings;
    public GameSettings gameSettings { get; private set; }
    public GameSettings DefaultSettings { get { return defaultSettings; } }
    float[][] screenResolution;

    public void StartManager()
    {
        LoadSettings();
        Application.quitting += SaveSettings;
        screenResolution = new float[][] {
            new float[] { Screen.currentResolution.width * 0.6f, Screen.currentResolution.height * 0.6f },
            new float[] { Screen.currentResolution.width * 0.8f, Screen.currentResolution.height * 0.8f },
            new float[] { Screen.currentResolution.width * 1f, Screen.currentResolution.height * 1f },
        };
        QualitySettings.SetQualityLevel((int)gameSettings.quality);
        Screen.SetResolution(
            (int)screenResolution[(int)gameSettings.quality][0],
            (int)screenResolution[(int)gameSettings.quality][1], true);
    }

    void OnEnable() => BroadcastMessages<bool>.AddListener(MessageType.PAUSE, OnPause);
    void OnDisable() => BroadcastMessages<bool>.RemoveListener(MessageType.PAUSE, OnPause);
    
    void OnPause(bool isPause)
    {
        if (isPause) SaveSettings();
    }

    ///<summary>
    ///Обновляет игровые настройки
    ///</summary>
    public void SetSettings(GameSettings _gameSettings)
    {
        gameSettings = _gameSettings;
        QualitySettings.SetQualityLevel((int)gameSettings.quality);
        int width = (int)screenResolution[(int)gameSettings.quality][0];
        int height = (int)screenResolution[(int)gameSettings.quality][1];
        Screen.SetResolution(
            (int)screenResolution[(int)gameSettings.quality][0],
            (int)screenResolution[(int)gameSettings.quality][1], true);
        Managers.audioManager.SetSettings(gameSettings);
    }

    void LoadSettings()
    {
        //загружаем настройки пользователя
        int savedSound, savedMusic;
        Quality quality;

        savedSound = PlayerPrefs.GetInt("Sound", Convert.ToInt32(defaultSettings.sound));
        savedMusic = PlayerPrefs.GetInt("Music", Convert.ToInt32(defaultSettings.music));
        quality = (Quality)PlayerPrefs.GetInt("Quality", Convert.ToInt32(defaultSettings.quality));

        GameSettings _gameSettings;
        _gameSettings.sound = Convert.ToBoolean(savedSound);
        _gameSettings.music = Convert.ToBoolean(savedMusic);
        _gameSettings.quality = quality;
        gameSettings = _gameSettings;
    }
    void SaveSettings()
    {
        // сохраняем настройки пользователя
        int savedSound = Convert.ToInt32(gameSettings.sound);
        int savedMusic = Convert.ToInt32(gameSettings.music);
        int savedQuality = Convert.ToInt32(gameSettings.quality);

        PlayerPrefs.SetInt("Sound", savedSound);
        PlayerPrefs.SetInt("Music", savedMusic);
        PlayerPrefs.SetInt("Quality", savedQuality);
        PlayerPrefs.Save();
    }
}
