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

    void OnEnable() => BroadcastMessages<bool>.AddListener(MessageType.PAUSE, IsPause);
    void OnDisable() => BroadcastMessages<bool>.RemoveListener(MessageType.PAUSE, IsPause);

    void IsPause(bool isPause)
    {
        pauseWindow.enabled = isPause;
        startGame.enabled = !isPause;
    }

    public void GoOn()
    {
        Managers.audioManager.PlaySound(tapSound);
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
        BroadcastMessages<bool>.SendMessage(MessageType.PAUSE, false);
    }
    public void MainMenu()
    {
        Managers.audioManager.PlaySound(tapSound);
        BroadcastMessages<bool>.SendMessage(MessageType.PAUSE, false);
        Managers.gameManager.LoadScene(1);
    }
    public void OpenSettings() => Managers.uiManager.OpenSettings(pauseWindow);
}
