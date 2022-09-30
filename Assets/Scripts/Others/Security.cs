using System;

namespace Assets.Scripts.Security
{
    ///<summary>
    ///Контейнер данных, зашифрованных с помощью 128-битного шифрования.
    ///Хранится в постоянной памяти
    ///</summary>
    [Serializable]
    public class EncryptedData
    {
        public string nickname;
        public float bestScore;
        public int health;
        public int money;
    }
    ///<summary>
    ///Контейнер данных, закодированных с помощью base-64 кодирования и XOR.
    ///Хранится во временной памяти
    ///</summary>
    [Serializable]
    public class EncodedData
    {
        public string nickname;
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

    ///<summary>
    ///Ключ шифрования
    ///</summary>
    public static class Key
    {
        public const string password = "dsjfnberqfleklevmlk";
    }
}
