using UnityEngine;
using System;

public class PlayerData : MonoBehaviour
{
    int revivalCostLast; // хранит предыдущее значение стоимости рестарта
    [Range(1, 1000)] public int moneyMultiplier;
    Timer timer;

    void Awake()
    {
        timer = gameObject.AddComponent<Timer>();
        revivalCostLast = 1;
    }

    void OnEnable()
    {
        BroadcastMessages.AddListener(Messages.RESTART, Restart);
        BroadcastMessages<string>.AddListener(Messages.COLLECT, CollectItem);
    }
    void OnDisable()
    {
        BroadcastMessages.RemoveListener(Messages.RESTART, Restart);
        BroadcastMessages<string>.RemoveListener(Messages.COLLECT, CollectItem);
        SetGameData();
    }
    
    void SetGameData()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        if (encodedData.score > encodedData.bestScore)
            encodedData.bestScore = encodedData.score;
        encodedData.score = 0f;
        encodedData.revivalCost = 1;
        encodedData.multiplierBonus = 1;
        GameManager.dataManager.SetGameData(encodedData);
    }

    public void SetScore(float playerSpeed)
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        encodedData.score += playerSpeed * Time.deltaTime * 10f * encodedData.multiplierBonus;
        int score = (int)encodedData.score;
        GameManager.dataManager.SetGameData(encodedData);
    }

    public void CollectItem(string tag)
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        if (tag == "Money")
            encodedData.money = encodedData.money + moneyMultiplier;
        else if (tag == "Multiplier")
        {
            int probability = UnityEngine.Random.Range(0, 100);
            if (probability > 95)
                encodedData.multiplierBonus = 5;
            else if (probability > 75)
                encodedData.multiplierBonus = 3;
            else
                encodedData.multiplierBonus = 2;
            timer.AddListener(10f, ResetMultiplier);
        }
        GameManager.dataManager.SetGameData(encodedData);
    }
    void ResetMultiplier()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        encodedData.multiplierBonus = 1;
        GameManager.dataManager.SetGameData(encodedData);
    }

    public void Restart()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        encodedData.health -= encodedData.revivalCost;
        { // каждое увеличение стоимости оживления соответствует числу Фибоначчи
            int revivalCostTemp = encodedData.revivalCost;
            encodedData.revivalCost += revivalCostLast;
            revivalCostLast = revivalCostTemp;
        }
        GameManager.dataManager.SetGameData(encodedData);
    }
}
