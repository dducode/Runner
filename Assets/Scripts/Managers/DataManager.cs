using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Common;

public class DataManager : MonoBehaviour, IManagers
{
    string jsonData;

    public void StartManager()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
    }

    public void SetGameData(EncodedData encodedData)
    {
        jsonData = JsonUtility.ToJson(encodedData);
        jsonData = B64X.Encode(jsonData);
    }
    public EncodedData GetGameData()
    {
        jsonData = B64X.Decode(jsonData);
        EncodedData encodedData = JsonUtility.FromJson<EncodedData>(jsonData);
        jsonData = B64X.Encode(jsonData);
        return encodedData;
    }

    public void SaveGameData()
    {
        // сохраняем игровые данные
        FileStream file = File.Create(Application.persistentDataPath + "/SaveGameData.dat");
        BinaryFormatter bf = new BinaryFormatter();
        SavedData data = new SavedData();

        jsonData = B64X.Decode(jsonData);
        EncodedData encodedData = JsonUtility.FromJson<EncodedData>(jsonData);
        EncryptedData encryptedData = new EncryptedData();
        encryptedData.bestScore = encodedData.bestScore;
        encryptedData.health = encodedData.health;
        encryptedData.money = encodedData.money;
        string jsonForSave = JsonUtility.ToJson(encryptedData);
        jsonForSave = AES.Encrypt(jsonForSave, Key.password);
        data.savedJson = jsonForSave;

        bf.Serialize(file, data);
        file.Close();
        jsonData = B64X.Encode(jsonData);
    }
    public void LoadGameData()
    {
        // загружаем игровые данные
        if (File.Exists(Application.persistentDataPath + "/SaveGameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveGameData.dat", FileMode.Open);
            SavedData data = (SavedData)bf.Deserialize(file);
            file.Close();

            string jsonForLoad = data.savedJson;
            jsonForLoad = AES.Decrypt(jsonForLoad, Key.password);
            EncryptedData encryptedData = JsonUtility.FromJson<EncryptedData>(jsonForLoad);
            EncodedData encodedData = new EncodedData();
            encodedData.bestScore = encryptedData.bestScore;
            encodedData.health = encryptedData.health;
            encodedData.money = encryptedData.money;
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
        }
        else
        {
            EncodedData encodedData = new EncodedData();
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
        }
    }
    public void ResetData()
    {
        // удаляем игровые данные
        if (File.Exists(Application.persistentDataPath + "/SaveGameData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveGameData.dat");

            EncodedData encodedData = new EncodedData();
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
        }
        else
            Debug.LogError("Сохранённые данные отсутствуют");
    }
}
