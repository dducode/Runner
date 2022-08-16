using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] Canvas pauseWindow;
    [SerializeField] TextMeshProUGUI startGame;
    [SerializeField] AudioClip tapSound;
    int time;

    void OnEnable() => BroadcastMessages<bool>.AddListener(Messages.PAUSE, IsPause);
    void OnDisable() => BroadcastMessages<bool>.RemoveListener(Messages.PAUSE, IsPause);

    void IsPause(bool isPause)
    {
        pauseWindow.enabled = isPause;
        startGame.enabled = !isPause;
    }

    public void GoOn()
    {
        GameManager.audioManager.PlaySound(tapSound);
        pauseWindow.enabled = false;
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
        BroadcastMessages<bool>.SendMessage(Messages.PAUSE, false);
    }
    public void MainMenu()
    {
        GameManager.audioManager.PlaySound(tapSound);
        BroadcastMessages<bool>.SendMessage(Messages.PAUSE, false);
        GameManager.gameManager.LoadScene(1);
    }
}
