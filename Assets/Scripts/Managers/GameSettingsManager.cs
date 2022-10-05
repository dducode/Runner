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
    int[][] screenResolution;

    public void StartManager()
    {
        LoadSettings();
        Application.quitting += SaveSettings;
        screenResolution = new int[][] {
            new int[] { (int)(Screen.currentResolution.width * 0.6f), (int)(Screen.currentResolution.height * 0.6f) },
            new int[] { (int)(Screen.currentResolution.width * 0.8f), (int)(Screen.currentResolution.height * 0.8f) },
            new int[] { Screen.currentResolution.width, Screen.currentResolution.height },
        };
        QualitySettings.SetQualityLevel((int)gameSettings.quality);
        Screen.SetResolution(
            screenResolution[(int)gameSettings.quality][0],
            screenResolution[(int)gameSettings.quality][1], true);
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

        int width = screenResolution[(int)gameSettings.quality][0];
        int height = screenResolution[(int)gameSettings.quality][1];
        Screen.SetResolution(
            screenResolution[(int)gameSettings.quality][0],
            screenResolution[(int)gameSettings.quality][1], true);
    }

    void LoadSettings()
    {
        //загружаем настройки пользователя
        gameSettings = new GameSettings(
            Convert.ToBoolean(PlayerPrefs.GetInt("Sound", Convert.ToInt32(defaultSettings.sound))), 
            Convert.ToBoolean(PlayerPrefs.GetInt("Music", Convert.ToInt32(defaultSettings.music))),
            (Quality)PlayerPrefs.GetInt("Quality", Convert.ToInt32(defaultSettings.quality))
        );
    }
    void SaveSettings()
    {
        // сохраняем настройки пользователя
        PlayerPrefs.SetInt("Sound", Convert.ToInt32(gameSettings.sound));
        PlayerPrefs.SetInt("Music", Convert.ToInt32(gameSettings.music));
        PlayerPrefs.SetInt("Quality", Convert.ToInt32(gameSettings.quality));
        PlayerPrefs.Save();
    }
}
