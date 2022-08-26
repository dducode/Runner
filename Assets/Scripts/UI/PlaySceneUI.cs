using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaySceneUI : MonoBehaviour, IUserInterface
{
    [SerializeField] Canvas deathWindow;
    [SerializeField] Canvas pauseWindow;
    [SerializeField] Button pauseButton;
    [SerializeField] AudioClip tapSound;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI moneys;
    [SerializeField] private TextMeshProUGUI multiplier;

    void OnEnable()
    {
        BroadcastMessages.AddListener(Messages.DEATH, Death);
        BroadcastMessages.AddListener(Messages.RESTART, Restart);
        BroadcastMessages<bool>.AddListener(Messages.PAUSE, IsPause);
    }
    void OnDisable()
    {
        BroadcastMessages.RemoveListener(Messages.DEATH, Death);
        BroadcastMessages.RemoveListener(Messages.RESTART, Restart);
        BroadcastMessages<bool>.RemoveListener(Messages.PAUSE, IsPause);
    }

    public void StartUI()
    {
        pauseWindow.enabled = false;
        deathWindow.enabled = false;
    }

    void Update()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        int scoreText = (int)encodedData.score;
        moneys.text = GameManager.uiManager.StringConversion(encodedData.money.ToString());
        score.text = GameManager.uiManager.StringConversion(scoreText.ToString());
        if (encodedData.multiplierBonus > 1)
            multiplier.text = "X" + encodedData.multiplierBonus;
        else
            multiplier.text = "";
    }

    public void Death()
    {
        StartCoroutine(AwaitDeathWindow());
        pauseButton.interactable = false;
    }
    void Restart() => pauseButton.interactable = true;

    IEnumerator AwaitDeathWindow()
    {
        yield return new WaitForSecondsRealtime(2);
        deathWindow.enabled = true;
    }

    public void Pause()
    {
        GameManager.audioManager.PlaySound(tapSound);
        BroadcastMessages<bool>.SendMessage(Messages.PAUSE, true);
    }
    public void IsPause(bool isPause)
    {
        pauseWindow.enabled = isPause;
        pauseButton.interactable = !isPause;
    }
}
