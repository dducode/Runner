using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerData : MonoBehaviour
{
    int revivalCostLast; // хранит предыдущее значение стоимости рестарта
    [SerializeField, Range(1, 1000)] int moneyMultiplier;
    [SerializeField, Range(1, 1000)] int scoreMultiplier;
    Timer timer;

    void Awake()
    {
        timer = gameObject.AddComponent<Timer>();
        revivalCostLast = 1;
    }

    void OnEnable()
    {
        BroadcastMessages.AddListener(Messages.RESTART, Restart);
    }
    void OnDisable()
    {
        BroadcastMessages.RemoveListener(Messages.RESTART, Restart);
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
        encodedData.score += playerSpeed * Time.deltaTime * 10f * encodedData.multiplierBonus * scoreMultiplier;
        int score = (int)encodedData.score;
        GameManager.dataManager.SetGameData(encodedData);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collectible>())
        {
            BonusType bonusType = other.GetComponent<Collectible>().BonusType;
            int bonusValue = other.GetComponent<Collectible>().BonusValue;
            CollectBonus(bonusType, bonusValue);
        }
    }

    public void CollectBonus(BonusType bonusType, int bonusValue)
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        if (bonusType is BonusType.Coin)
            encodedData.money = encodedData.money + bonusValue * moneyMultiplier;
        else if (bonusType is BonusType.Multiplier)
        {
            encodedData.multiplierBonus = bonusValue;
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
