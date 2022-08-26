using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class MainSceneUI : MonoBehaviour, IUserInterface
{
    [SerializeField] Canvas buyHealthWindow;
    [SerializeField] Canvas helpWindow;
    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] GameObject health;
    [SerializeField] GameObject moneys;
    [SerializeField] AudioClip tapSound;
    [SerializeField] List<Button> buyButtons;
    TextMeshProUGUI healthText;
    TextMeshProUGUI moneysText;
    Canvas mainWindow;

    public void StartUI()
    {
        buyHealthWindow.enabled = false;
        helpWindow.enabled = false;
        mainWindow = GetComponent<Canvas>();
        healthText = health.GetComponentInChildren<TextMeshProUGUI>();
        moneysText = moneys.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        int score = (int)encodedData.bestScore;
        healthText.text = 
            GameManager.uiManager.StringConversion(encodedData.health.ToString());
        bestScore.text = 
            "Best Score: " + GameManager.uiManager.StringConversion(score.ToString());
        moneysText.text = 
            GameManager.uiManager.StringConversion(encodedData.money.ToString());
    }

    public void StartGame()
    {
        GameManager.audioManager.PlaySound(tapSound);
        GameManager.gameManager.LoadScene(2);
    }
    public void BuyHealth()
    {
        GameManager.audioManager.PlaySound(tapSound);
        buyHealthWindow.enabled = true;
        mainWindow.enabled = false;
        health.transform.SetParent(buyHealthWindow.transform);
        moneys.transform.SetParent(buyHealthWindow.transform);
        EncodedData encodedData = GameManager.dataManager.GetGameData();
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
        GameManager.audioManager.PlaySound(tapSound);
        helpWindow.enabled = true;
        mainWindow.enabled = false;
    }
    public void CloseBuyWindow()
    {
        GameManager.audioManager.PlaySound(tapSound);
        buyHealthWindow.enabled = false;
        mainWindow.enabled = true;
        health.transform.SetParent(transform);
        moneys.transform.SetParent(transform);
    }
    public void CloseHelpWindow()
    {
        GameManager.audioManager.PlaySound(tapSound);
        helpWindow.enabled = false;
        mainWindow.enabled = true;
    }
    public void OpenSettings() => GameManager.uiManager.OpenSettings(mainWindow);

    public void HealthAdd(int healthes)
    {
        GameManager.audioManager.PlaySound(tapSound);
        EncodedData encodedData = GameManager.dataManager.GetGameData();
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
        GameManager.dataManager.SetGameData(encodedData);
        GameManager.dataManager.SaveGameData();
    }
}
