using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class MyDataBase
{
    const string fileName = "db.bytes";
    static string DBPath;

    static MyDataBase()
    {
        DBPath = GetDatabasePath();
    }

    static string GetDatabasePath()
    {
#if UNITY_EDITOR
        return Path.Combine(Application.streamingAssetsPath, fileName);
#elif UNITY_ANDROID
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if(!File.Exists(filePath)) UnpackDatabase(filePath);
        return filePath;
#endif
    }

    static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) {}

        File.WriteAllBytes(toPath, reader.bytes);
    }

    public static void ExecuteQueryWithoutAnswer(string query)
    {
        using var connection = new SqliteConnection("Data Source=" + DBPath);
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);
        command.ExecuteNonQuery();
    }

    public static string ExecuteQueryWithAnswer(string query)
    {
        using var connection = new SqliteConnection("Data Source=" + DBPath);
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);
        var answer = command.ExecuteScalar();
        return answer?.ToString();
    }

    public static DataTable GetTable(string query)
    {
        using var connection = new SqliteConnection("Data Source=" + DBPath);
        connection.Open();

        using SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);
        DataSet ds = new DataSet();
        adapter.Fill(ds);

        return ds.Tables[0];
    }
}
