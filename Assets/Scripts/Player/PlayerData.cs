using UnityEngine;
using System;
using Assets.Scripts.Security;

[RequireComponent(typeof(PlayerController))]
public class PlayerData : MonoBehaviour
{
    int revivalCostLast; // хранит предыдущее значение стоимости рестарта
    Timer timer;

    void Awake()
    {
        timer = gameObject.AddComponent<Timer>();
        revivalCostLast = 1;
    }

    void OnEnable()
    {
        BroadcastMessages.AddListener(MessageType.RESTART, Restart);
        EncodedData encodedData = Managers.dataManager.GetData();
        encodedData.score = 0f;
        encodedData.revivalCost = 1;
        encodedData.multiplierBonus = 1;
        Managers.dataManager.SetData(encodedData);
    }
    void OnDisable()
    {
        BroadcastMessages.RemoveListener(MessageType.RESTART, Restart);
        EncodedData encodedData = Managers.dataManager.GetData();
        if (encodedData.score > encodedData.bestScore)
            encodedData.bestScore = encodedData.score;
        Managers.dataManager.SetData(encodedData, true);
    }

    public void SetScore(float playerSpeed)
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        encodedData.score += playerSpeed * Time.deltaTime * 10f * encodedData.multiplierBonus;
        int score = (int)encodedData.score;
        Managers.dataManager.SetData(encodedData);
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
        EncodedData encodedData = Managers.dataManager.GetData();
        if (bonusType is BonusType.Coin)
            encodedData.money = encodedData.money + bonusValue;
        else if (bonusType is BonusType.Multiplier)
        {
            encodedData.multiplierBonus = bonusValue;
            timer.AddListener(10f, ResetMultiplier);
        }
        Managers.dataManager.SetData(encodedData);
    }
    void ResetMultiplier()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        encodedData.multiplierBonus = 1;
        Managers.dataManager.SetData(encodedData);
    }

    public void Restart()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        encodedData.health -= encodedData.revivalCost;
        { // каждое увеличение стоимости оживления соответствует числу Фибоначчи
            int revivalCostTemp = encodedData.revivalCost;
            encodedData.revivalCost += revivalCostLast;
            revivalCostLast = revivalCostTemp;
        }
        Managers.dataManager.SetData(encodedData);
    }
}
