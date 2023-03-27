using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;



///<summary>
///Класс, управляющий пользовательским интерфейсом в главной сцене
///</summary>
[RequireComponent(typeof(Canvas))]
public class MainSceneUI : MonoBehaviour, IUserInterface {

    [SerializeField]
    private Canvas buyHealthWindow; // Окно покупки здоровья

    [SerializeField]
    private Canvas helpWindow; // Окно навигации по игре

    [SerializeField]
    private Canvas tournamentTable; // Таблица еженедельного турнира

    [SerializeField]
    private TextMeshProUGUI bestScore;

    [SerializeField]
    private RectTransform health; // Отображение здоровья

    [SerializeField]
    private RectTransform moneys; // Отображение валюты

    [SerializeField]
    private AudioClip tapSound; // Звук нажатия кнопки UI

    [SerializeField]
    private List<Button> buyButtons; // Кнопки покупки здоровья

    [SerializeField]
    private Button startGame; // Кнопка старта игры

    [SerializeField]
    private TMP_InputField nickname; // Поле ввода никнейма игрока

    [SerializeField]
    private Canvas warning;

    private TournamentTable m_table;
    private TextMeshProUGUI m_healthText;
    private TextMeshProUGUI m_moneysText;


    public void StartUI () {
        nickname.gameObject.SetActive(false);
        GetComponent<Canvas>();
        m_healthText = health.GetComponentInChildren<TextMeshProUGUI>();
        m_moneysText = moneys.GetComponentInChildren<TextMeshProUGUI>();
        m_table = tournamentTable.GetComponentInChildren<TournamentTable>();
        var encodedData = Managers.dataManager.GetData();

        if (encodedData.nickname == "") {
            nickname.gameObject.SetActive(true);
            startGame.interactable = false;
        }
        else
            m_table.InitializeTable();

        UpdateViews();
    }


    public void WriteNick () {
        if (nickname.textComponent.text.Length < 3) {
            ShowWarning();
            return;
        }

        var encodedData = Managers.dataManager.GetData();
        encodedData.nickname = nickname.textComponent.text;
        Managers.dataManager.SetData(encodedData, true);
        m_table.AddPlayerInTable(encodedData.nickname);
        nickname.gameObject.SetActive(false);
        startGame.interactable = true;
    }


    public void UpdateViews () {
        var encodedData = Managers.dataManager.GetData();
        var score = (int) encodedData.bestScore;
        m_healthText.text =
            Managers.uiManager.AddSeparator(encodedData.health.ToString());
        bestScore.text =
            "Best Score: " + Managers.uiManager.AddSeparator(score.ToString());
        m_moneysText.text =
            Managers.uiManager.AddSeparator(encodedData.money.ToString());
        m_table.UpdatePlayerScore();
    }


    public void StartGame () {
        Managers.audioManager.PlaySound(tapSound);
        Managers.gameManager.LoadScene(2);
    }


    public void BuyHealth () {
        health.SetParent(buyHealthWindow.transform);
        moneys.SetParent(buyHealthWindow.transform);
        var encodedData = Managers.dataManager.GetData();

        foreach (var buyButton in buyButtons) {
            var amount = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            var cost = int.Parse(amount.text);
            cost *= 100;
            buyButton.interactable = encodedData.money > cost;
        }
    }


    public void CloseBuyWindow () {
        health.SetParent(transform);
        moneys.SetParent(transform);
    }


    public void HealthAdd (int healthes) {
        Managers.audioManager.PlaySound(tapSound);
        var encodedData = Managers.dataManager.GetData();
        var cost = healthes * 100;
        encodedData.health += healthes;
        encodedData.money -= cost;

        foreach (var buyButton in buyButtons) {
            var amount = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            var price = int.Parse(amount.text);
            price *= 100;
            buyButton.interactable = encodedData.money >= price;
        }

        Managers.dataManager.SetData(encodedData, true);
        UpdateViews();
    }


    public void HideWarning () {
        warning.enabled = false;
    }


    private void ShowWarning () {
        warning.enabled = true;
    }

}