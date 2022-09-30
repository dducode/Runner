using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.Runtime;
using TMPro;
using Assets.Scripts.Security;

public class TournamentTable : MonoBehaviour
{
    [SerializeField] List<GameObject> rows;

    public void AddPlayerInTable(string nickname)
    {
        string query = $"INSERT INTO Players (nickname) VALUES ('{nickname}')";
        Managers.databaseManager.ExecuteQueryWithoutAnswer(query);
        string initialQuery = "UPDATE Players SET best_score = 0";
        Managers.databaseManager.ExecuteQueryWithoutAnswer(initialQuery);
        UpdateScores(1);
        InitializeTable();
    }

    public void InitializeTable()
    {
        int day = PlayerPrefs.GetInt("LastDay", Convert.ToInt32(DateTime.Today.DayOfWeek));
        int today = Convert.ToInt32(DateTime.Today.DayOfWeek);
        if (today > day)
        {
            UpdateScores(today - day);
        }
        else if (today < day)
        {
            string initialQuery = "UPDATE Players SET best_score = 0";
            Managers.databaseManager.ExecuteQueryWithoutAnswer(initialQuery);
            UpdateScores(today);
        }
        DataTable table = Managers.databaseManager.GetTable("SELECT * FROM Players ORDER BY best_score DESC");
        for (int i = 0; i < table.Rows.Count; i++)
            for (int j = 1; j < table.Columns.Count; j++)
                rows[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = table.Rows[i][j].ToString();
        PlayerPrefs.SetInt("LastDay", today);
        PlayerPrefs.Save();
    }

    ///<summary>
    ///Обновляет счёт игрока в базе данных
    ///</summary>
    public void UpdatePlayerScore()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        string query = @$"
        SELECT best_score
        FROM Players
        WHERE nickname = '{encodedData.nickname}'
        ";
        int score = Convert.ToInt32(Managers.databaseManager.ExecuteQueryWithAnswer(query));
        if (encodedData.score > score)
        {
            string _query = @$"
            UPDATE Players 
            SET best_score = {(int)encodedData.score} 
            WHERE nickname = '{encodedData.nickname}'
            ";
            Managers.databaseManager.ExecuteQueryWithoutAnswer(_query);
        }
        DataTable table = Managers.databaseManager.GetTable("SELECT * FROM Players ORDER BY best_score DESC");
        for (int i = 0; i < table.Rows.Count; i++)
            for (int j = 1; j < table.Columns.Count; j++)
                rows[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = table.Rows[i][j].ToString();
        for (int i = 0; i < rows.Count; i++)
        {
            TextMeshProUGUI weeklyBestScore = rows[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            weeklyBestScore.text = Managers.uiManager.StringConversion(weeklyBestScore.text);
        }
    }

    void UpdateScores(int day)
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        for (int i = 0; i < day; i++)
        {
            DataTable table = Managers.databaseManager.GetTable("SELECT * FROM Players ORDER BY id_player");
            for (int j = 0; j < table.Rows.Count; j++)
            {
                int score = UnityEngine.Random.Range(0, 1000000);
                if (encodedData.nickname == table.Rows[j][1].ToString())
                {
                    string query = @$"
                    UPDATE Players 
                    SET best_score = {(int)encodedData.bestScore}
                    WHERE id_player = {j + 1}";
                    Managers.databaseManager.ExecuteQueryWithoutAnswer(query);
                }
                else if (score > Convert.ToInt32(table.Rows[j][2]))
                {
                    string query = @$"
                    UPDATE Players 
                    SET best_score = {score}
                    WHERE id_player = {j + 1}";
                    Managers.databaseManager.ExecuteQueryWithoutAnswer(query);
                }
            }
        }
    }
}
