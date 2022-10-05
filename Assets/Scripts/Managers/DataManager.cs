using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Common;
using Assets.Scripts.Security;
using System;
using System.Data;

///<summary>
///Класс для доступа к данным игрока
///</summary>
public class DataManager : MonoBehaviour, IManagers
{
    string jsonData; // данные игрока в формате JSON

    public void StartManager()
    {
        LoadData();
        Application.quitting += SaveData;
    }

    ///<summary>
    ///Обновляет данные игрока
    ///</summary>
    ///<param name="serialize">Указывает, должны ли данные сохраняться на диск</param>
    public void SetData(EncodedData encodedData, bool serialize = false)
    {
        jsonData = JsonUtility.ToJson(encodedData);
        jsonData = B64X.Encode(jsonData);
        if (serialize) SaveData();
    }
    ///<returns>Данные игрока</returns>
    public EncodedData GetData()
    {
        jsonData = B64X.Decode(jsonData);
        EncodedData encodedData = JsonUtility.FromJson<EncodedData>(jsonData);
        jsonData = B64X.Encode(jsonData);
        return encodedData;
    }

    void SaveData()
    {
        // сохраняем игровые данные
        FileStream file = File.Create(Application.persistentDataPath + "/SaveGameData.dat");
        BinaryFormatter bf = new BinaryFormatter();
        SavedData data = new SavedData();

        jsonData = B64X.Decode(jsonData);
        EncodedData encodedData = JsonUtility.FromJson<EncodedData>(jsonData);
        EncryptedData encryptedData = new EncryptedData();
        encryptedData.nickname = encodedData.nickname;
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
    void LoadData()
    {
        // загружаем игровые данные
        if (File.Exists(Application.persistentDataPath + "/SaveGameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveGameData.dat", FileMode.Open);
            SavedData data = bf.Deserialize(file) as SavedData;
            file.Close();

            string jsonForLoad = data.savedJson;
            jsonForLoad = AES.Decrypt(jsonForLoad, Key.password);
            EncryptedData encryptedData = JsonUtility.FromJson<EncryptedData>(jsonForLoad);
            EncodedData encodedData = new EncodedData();
            encodedData.nickname = encryptedData.nickname;
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

#if UNITY_EDITOR

    bool isDev;
    Rect window = new Rect(0, Screen.height - 150, 300, 150);

    void ResetData()
    {
        // удаляем игровые данные
        if (File.Exists(Application.persistentDataPath + "/SaveGameData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveGameData.dat");

            EncodedData encodedData = GetData();
            // удаляем данные игрока из базы данных
            Managers.databaseManager.ExecuteQueryWithoutAnswer(
                $"DELETE FROM Players WHERE nickname = '{encodedData.nickname}'"
                );
            Managers.databaseManager.ExecuteQueryWithoutAnswer("UPDATE Players SET best_score = 0");
            encodedData = new EncodedData();
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
            
            Managers.uiManager.UpdateViews();
        }
        else
            Debug.LogError("Сохранённые данные отсутствуют");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            isDev = !isDev;
    }

    void OnGUI()
    {
        if (isDev)
            window = GUILayout.Window(0, window, Window, "Development console");  
    }

    void Window(int id)
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        GUILayoutOption[] options = new GUILayoutOption[] { 
            GUILayout.MaxWidth(400), GUILayout.MaxHeight(400),
        };
        style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.green;
        if (GUILayout.Button("100 000 money", style, options))
        {
            jsonData = B64X.Decode(jsonData);
            EncodedData encodedData = JsonUtility.FromJson<EncodedData>(jsonData);
            encodedData.money += 100000;
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
            SaveData();
        }
        if (GUILayout.Button("1 000 000 score", style, options))
        {
            jsonData = B64X.Decode(jsonData);
            EncodedData encodedData = JsonUtility.FromJson<EncodedData>(jsonData);
            encodedData.bestScore += 1000000;
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
            SaveData();
            Managers.uiManager.UpdateViews();
        }
        style.normal.textColor = Color.red;
        if (GUILayout.Button("Reset Data", style, options))
            ResetData();
        GUI.DragWindow();
    }
#endif
}
