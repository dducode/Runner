using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

///<summary>
///Класс для доступа к базе данных
///</summary>
public class DatabaseManager : MonoBehaviour, IManagers
{
    const string fileName = "dbAndroid.bytes";
    string DBPath;

    public void StartManager()
    {
        DBPath = GetDatabasePath();
    }

    string GetDatabasePath()
    {
#if UNITY_EDITOR
        return "Assets/DataBase/dbEditor.bytes";
#elif UNITY_ANDROID
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if(!File.Exists(filePath)) UnpackDatabase(filePath);
        return filePath;
#endif
    }

    void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) {}

        File.WriteAllBytes(toPath, reader.bytes);
    }

    ///<summary>
    ///Выполняет запрос к базе данных
    ///</summary>
    public void ExecuteQueryWithoutAnswer(string query)
    {
        using var connection = new SqliteConnection("Data Source=" + DBPath);
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);
        command.ExecuteNonQuery();
    }

    ///<summary>
    ///Выполняет запрос к базе данных
    ///</summary>
    ///<returns>Результат запроса</returns>
    public object ExecuteQueryWithAnswer(string query)
    {
        using var connection = new SqliteConnection("Data Source=" + DBPath);
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);
        var answer = command.ExecuteScalar();
        return answer;
    }

    ///<returns>Таблица из базы данных</returns>
    public DataTable GetTable(string query)
    {
        using var connection = new SqliteConnection("Data Source=" + DBPath);
        connection.Open();

        using SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);
        DataSet ds = new DataSet();
        adapter.Fill(ds);

        return ds.Tables[0];
    }
}
