using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.Runtime;
using TMPro;
using Assets.Scripts.Security;
using UnityRandom = UnityEngine.Random;

public class TournamentTable : MonoBehaviour
{
    [SerializeField] List<GameObject> rows;
    [SerializeField] Prizes[] prizesForWinning;

    ///<summary>
    ///Добавляет нового игрока в общую таблицу.
    ///Вызывается в самом начале игры и только один раз за все время
    ///</summary>
    public void AddPlayerInTable(string nickname)
    {
        string query = $@"
        INSERT INTO Players (nickname) VALUES ('{nickname}');
        INSERT INTO Dates (Last_date, Next_date)
        VALUES 
        (strftime('%s', 'now'), 
        strftime('%s', date ('now'), 'weekday 0'))";
        Managers.databaseManager.ExecuteQueryWithoutAnswer(query);
        UpdateScores(Convert.ToInt32(DateTime.Today.DayOfWeek) + 1);
        UpdateTable();
    }

    public void InitializeTable()
    {
        string query = "UPDATE Dates SET Last_date = strftime('%s', 'now')";
        Managers.databaseManager.ExecuteQueryWithoutAnswer(query);
        DataTable dates = Managers.databaseManager.GetTable("SELECT * FROM Dates");
        int lastDate = Convert.ToInt32(dates.Rows[0][1]); // дата последнего входа в игру
        int nextDate = Convert.ToInt32(dates.Rows[0][2]); // дата начала следующего турнира (каждое воскресенье)
        int lastDay = PlayerPrefs.GetInt(
            "LastDay", Convert.ToInt32(DateTime.Today.DayOfWeek)); // день недели последнего входа в игру

        if (lastDate > nextDate) // если турнир закончился
        {
            UpdateScores(6 - lastDay); // обновляем очки других игроков за прошедшие дни прошлого турнира
            DetermineWinners();
            query = @"
            UPDATE Dates SET Next_date = strftime('%s', date ('now', '1 days'), 'weekday 0');
            UPDATE Players SET best_score = 0"; // устанавливаем дату окончания турнира (след. воскресенье)
            Managers.databaseManager.ExecuteQueryWithoutAnswer(query);

            // обновляем очки других игроков за прошедшие дни с начала текущего турнира + сегодняшний день
            UpdateScores(Convert.ToInt32(DateTime.Today.DayOfWeek) + 1);
        }
        // обновляем очки других игроков за прошедшие дни текущего турнира со дня последнего входа
        else UpdateScores(Convert.ToInt32(DateTime.Today.DayOfWeek) - lastDay);

        UpdateTable();

        PlayerPrefs.SetInt("LastDay", Convert.ToInt32(DateTime.Today.DayOfWeek));
        PlayerPrefs.Save();
    }

    void UpdateTable()
    {
        DataTable table = Managers.databaseManager.GetTable("SELECT * FROM Players ORDER BY best_score DESC");
        for (int i = 0; i < table.Rows.Count; i++)
            for (int j = 1; j < table.Columns.Count; j++)
                rows[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = table.Rows[i][j].ToString();
        for (int i = 0; i < rows.Count; i++)
        {
            TextMeshProUGUI weeklyBestScore = rows[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            weeklyBestScore.text = Managers.uiManager.AddSeparator(weeklyBestScore.text);
        }
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
            query = @$"
            UPDATE Players 
            SET best_score = {(int)encodedData.score} 
            WHERE nickname = '{encodedData.nickname}'
            ";
            Managers.databaseManager.ExecuteQueryWithoutAnswer(query);
        }
        UpdateTable();
    }

    void UpdateScores(int day)
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        for (int i = 0; i < day; i++)
        {
            DataTable table = Managers.databaseManager.GetTable("SELECT * FROM Players ORDER BY id_player");
            for (int j = 0; j < table.Rows.Count; j++)
            {
                int score = UnityRandom.Range(0, 1000000);
                if (encodedData.nickname == table.Rows[j][1].ToString())
                    continue;
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

    void DetermineWinners()
    {
        string query = "SELECT * FROM Players ORDER BY best_score DESC";
        DataTable table = Managers.databaseManager.GetTable(query);
        EncodedData encodedData = Managers.dataManager.GetData();
        for (int i = 0; i < prizesForWinning.Length; i++)
        {
            if (encodedData.nickname == table.Rows[1][i].ToString())
            {
                encodedData.money += prizesForWinning[i].Moneys;
                encodedData.health += prizesForWinning[i].Health;
            }
        }
    }
}
