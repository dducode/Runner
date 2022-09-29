using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.Runtime;
using TMPro;

public class TournamentTable : MonoBehaviour
{
    [SerializeField] List<GameObject> rows;

    public void AddPlayerInTable(string nickname)
    {
        string query = $"INSERT INTO Players (nickname) VALUES ('{nickname}')";
        MyDataBase.ExecuteQueryWithoutAnswer(query);
        string initialQuery = "UPDATE Players SET best_score = 0";
        MyDataBase.ExecuteQueryWithoutAnswer(initialQuery);
        GameManager.dataManager.UpdateScoresInDatabase(1);
        InitializeTable();
    }

    public void InitializeTable()
    {
        int day = PlayerPrefs.GetInt("LastDay", Convert.ToInt32(DateTime.Today.DayOfWeek));
        int today = Convert.ToInt32(DateTime.Today.DayOfWeek);
        if (today > day)
        {
            GameManager.dataManager.UpdateScoresInDatabase(today - day);
        }
        else if (today < day)
        {
            string initialQuery = "UPDATE Players SET best_score = 0";
            MyDataBase.ExecuteQueryWithoutAnswer(initialQuery);
            GameManager.dataManager.UpdateScoresInDatabase(today);
        }
        DataTable table = MyDataBase.GetTable("SELECT * FROM Players ORDER BY best_score DESC");
        for (int i = 0; i < table.Rows.Count; i++)
            for (int j = 1; j < table.Columns.Count; j++)
                rows[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = table.Rows[i][j].ToString();
        PlayerPrefs.SetInt("LastDay", today);
    }

    public void UpdatePlayerScoreInDatabase()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        if (encodedData.bestScore > 0)
        {
            string query = @$"
            UPDATE Players 
            SET best_score = {(int)encodedData.bestScore} 
            WHERE nickname = '{encodedData.nickname}'
            ";
            MyDataBase.ExecuteQueryWithoutAnswer(query);
        }
        DataTable table = MyDataBase.GetTable("SELECT * FROM Players ORDER BY best_score DESC");
        for (int i = 0; i < table.Rows.Count; i++)
            for (int j = 1; j < table.Columns.Count; j++)
                rows[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = table.Rows[i][j].ToString();
        for (int i = 0; i < rows.Count; i++)
        {
            TextMeshProUGUI bestScore = rows[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            bestScore.text = GameManager.uiManager.StringConversion(bestScore.text);
        }
    }
}
