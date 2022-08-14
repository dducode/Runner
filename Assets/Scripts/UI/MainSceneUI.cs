using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneUI : MonoBehaviour
{
    [SerializeField] Canvas buyHealthWindow;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] TextMeshProUGUI moneys;
    [SerializeField] AudioClip tapSound;

    public void StartUI()
    {
        buyHealthWindow.enabled = false;
    }

    void Update()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        int score = (int)encodedData.bestScore;
        health.text = encodedData.health.ToString();
        bestScore.text = "Best Score: " + score;
        moneys.text = encodedData.money.ToString();
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
    }
    public void Close()
    {
        GameManager.audioManager.PlaySound(tapSound);
        buyHealthWindow.enabled = false;
    }

    public void HealthAdd(int value)
    {
        GameManager.audioManager.PlaySound(tapSound);
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        if (encodedData.money >= value * 100)
        {
            encodedData.health = encodedData.health + value;
            encodedData.money = encodedData.money - value * 100;
        }
        GameManager.dataManager.SetGameData(encodedData);
        GameManager.dataManager.SaveGameData();
    }
}
