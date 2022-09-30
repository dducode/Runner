using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using Assets.Scripts.Security;

///<summary>
///Класс, управляющий пользовательским интерфейсом в главной сцене
///</summary>
public class MainSceneUI : MonoBehaviour, IUserInterface
{
    [SerializeField] Canvas buyHealthWindow; // Окно покупки здоровья
    [SerializeField] Canvas helpWindow; // Окно навигации по игре
    [SerializeField] GameObject tournamentTable; // Таблица еженедельного турнира
    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] GameObject health; // Отображение здоровья
    [SerializeField] GameObject moneys; // Отображение валюты
    [SerializeField] AudioClip tapSound; // Звук нажатия кнопки UI
    [SerializeField] List<Button> buyButtons; // Кнопки покупки здоровья
    [SerializeField] Button startGame; // Кнопка старта игры
    [SerializeField] TMP_InputField nickname; // Поле ввода никнейма игрока
    TournamentTable table;
    TextMeshProUGUI healthText;
    TextMeshProUGUI moneysText;
    Canvas mainWindow;

    public void StartUI()
    {
        nickname.gameObject.SetActive(false);
        tournamentTable.SetActive(false);
        buyHealthWindow.enabled = false;
        helpWindow.enabled = false;
        mainWindow = GetComponent<Canvas>();
        healthText = health.GetComponentInChildren<TextMeshProUGUI>();
        moneysText = moneys.GetComponentInChildren<TextMeshProUGUI>();
        table = tournamentTable.GetComponent<TournamentTable>();
        EncodedData encodedData = Managers.dataManager.GetData();
        if (encodedData.nickname == "")
        {
            nickname.gameObject.SetActive(true);
            startGame.interactable = false;
        }
        else
            table.InitializeTable();
        UpdateViews();
    }

    public void WriteNick()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        encodedData.nickname = nickname.textComponent.text;
        Managers.dataManager.SetData(encodedData);
        table.AddPlayerInTable(encodedData.nickname);
        nickname.gameObject.SetActive(false);
        startGame.interactable = true;
    }

    public void UpdateViews()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        int score = (int)encodedData.bestScore;
        healthText.text = 
            Managers.uiManager.StringConversion(encodedData.health.ToString());
        bestScore.text = 
            "Best Score: " + Managers.uiManager.StringConversion(score.ToString());
        moneysText.text = 
            Managers.uiManager.StringConversion(encodedData.money.ToString());
        table.UpdatePlayerScore();
    }

    public void StartGame()
    {
        Managers.audioManager.PlaySound(tapSound);
        Managers.gameManager.LoadScene(2);
    }
    public void BuyHealth()
    {
        Managers.audioManager.PlaySound(tapSound);
        buyHealthWindow.enabled = true;
        mainWindow.enabled = false;
        health.transform.SetParent(buyHealthWindow.transform);
        moneys.transform.SetParent(buyHealthWindow.transform);
        EncodedData encodedData = Managers.dataManager.GetData();
        foreach (Button buyButton in buyButtons)
        {
            TextMeshProUGUI amount = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            int cost = Int32.Parse(amount.text);
            cost *= 100;
            buyButton.interactable = encodedData.money > cost;
        }
    }
    public void Help()
    {
        Managers.audioManager.PlaySound(tapSound);
        helpWindow.enabled = true;
        mainWindow.enabled = false;
    }
    public void OpenTable()
    {
        Managers.audioManager.PlaySound(tapSound);
        tournamentTable.SetActive(!tournamentTable.activeSelf);
    }
    public void CloseBuyWindow()
    {
        Managers.audioManager.PlaySound(tapSound);
        buyHealthWindow.enabled = false;
        mainWindow.enabled = true;
        health.transform.SetParent(transform);
        moneys.transform.SetParent(transform);
    }
    public void CloseHelpWindow()
    {
        Managers.audioManager.PlaySound(tapSound);
        helpWindow.enabled = false;
        mainWindow.enabled = true;
    }
    public void OpenSettings() => Managers.uiManager.OpenSettings(mainWindow);

    public void HealthAdd(int healthes)
    {
        Managers.audioManager.PlaySound(tapSound);
        EncodedData encodedData = Managers.dataManager.GetData();
        int cost = healthes * 100;
        encodedData.health = encodedData.health + healthes;
        encodedData.money = encodedData.money - cost;
        foreach (Button buyButton in buyButtons)
        {
            TextMeshProUGUI amount = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            int price = Int32.Parse(amount.text);
            price *= 100;
            buyButton.interactable = encodedData.money >= price;
        }
        Managers.dataManager.SetData(encodedData, true);
        UpdateViews();
    }
}
