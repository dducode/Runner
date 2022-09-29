using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Common;
using System;
using System.Data;

public class DataManager : MonoBehaviour, IManagers
{
    string jsonData;

    public void StartManager()
    {
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
    public void LoadGameData()
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

    public void UpdateScoresInDatabase(int day)
    {
        EncodedData encodedData = GetGameData();
        for (int i = 0; i < day; i++)
        {
            DataTable table = MyDataBase.GetTable("SELECT * FROM Players ORDER BY id_player");
            for (int j = 0; j < table.Rows.Count; j++)
            {
                int score = UnityEngine.Random.Range(0, 1000000);
                if (encodedData.nickname == table.Rows[j][1].ToString())
                {
                    string query = @$"
                    UPDATE Players 
                    SET best_score = {(int)encodedData.bestScore}
                    WHERE id_player = {j + 1}";
                    MyDataBase.ExecuteQueryWithoutAnswer(query);
                }
                else if (score > Convert.ToInt32(table.Rows[j][2]))
                {
                    string query = @$"
                    UPDATE Players 
                    SET best_score = {score}
                    WHERE id_player = {j + 1}";
                    MyDataBase.ExecuteQueryWithoutAnswer(query);
                }
            }
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

            EncodedData encodedData = new EncodedData();
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
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
            SaveGameData();
        }
        if (GUILayout.Button("1 000 000 score", style, options))
        {
            jsonData = B64X.Decode(jsonData);
            EncodedData encodedData = JsonUtility.FromJson<EncodedData>(jsonData);
            encodedData.bestScore += 1000000;
            jsonData = JsonUtility.ToJson(encodedData);
            jsonData = B64X.Encode(jsonData);
            SaveGameData();
            GameManager.uiManager.UpdateViews();
        }
        style.normal.textColor = Color.red;
        if (GUILayout.Button("Reset Data", style, options))
            ResetData();
        GUI.DragWindow();
    }
#endif
}
