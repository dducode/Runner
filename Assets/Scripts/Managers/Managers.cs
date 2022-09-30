using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Класс доступа к менеджерам
///</summary>
public class Managers : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }
    public static AudioManager audioManager { get; private set; }
    public static DataManager dataManager { get; private set; }
    public static UIManager uiManager { get; private set; }
    public static DatabaseManager databaseManager { get; private set; }
    public static GameSettingsManager settingsManager { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameManager = GetComponentInChildren<GameManager>();
        audioManager = GetComponentInChildren<AudioManager>();
        dataManager = GetComponentInChildren<DataManager>();
        uiManager = GetComponentInChildren<UIManager>();
        databaseManager = GetComponentInChildren<DatabaseManager>();
        settingsManager = GetComponentInChildren<GameSettingsManager>();

        dataManager.StartManager();
        settingsManager.StartManager();
        databaseManager.StartManager();
        audioManager.StartManager();
        uiManager.StartManager();
        gameManager.StartManager();
    }
}
