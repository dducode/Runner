using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI startGame;
    int time;

    void Start() => startGame.enabled = false;

    public void GoOn()
    {
        startGame.enabled = true;
        time = 3;
        StartCoroutine(StartTimer());
    }
    IEnumerator StartTimer()
    {
        while (time > 0)
        {
            startGame.text = time.ToString();
            yield return new WaitForSecondsRealtime(1);
            --time;
        }
        BroadcastMessages<bool>.SendMessage(MessageType.PAUSE, false);
        startGame.enabled = false;
    }
    public void MainMenu()
    {
        Managers.gameManager.LoadScene(1);
    }
}
