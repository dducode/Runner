using System;

[Serializable]
public class EncryptedData
{
    public float bestScore;
    public int health;
    public int money;
}
[Serializable]
public class EncodedData
{
    public float score;
    public float bestScore;
    public int health;
    public int money;
    public int revivalCost;
    public int multiplierBonus;

    public EncodedData()
    {
        revivalCost = 1;
        multiplierBonus = 1;
    }
}

public struct Key
{
    public const string password = "dsjfnberqfleklevmlk";
}
