using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaySceneUI : MonoBehaviour
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

    void Awake()
    {
        pauseWindow.enabled = false;
        deathWindow.enabled = false;
    }

    void Update()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        int scoreText = (int)encodedData.score;
        moneys.text = encodedData.money.ToString();
        score.text = scoreText.ToString();
        if (encodedData.multiplierBonus > 1)
            multiplier.text = "X" + encodedData.multiplierBonus;
        else
            multiplier.text = "";
    }

    public void Death()
    {
        StartCoroutine(AwaitDeathWindow());
        pauseButton.enabled = false;
    }
    IEnumerator AwaitDeathWindow()
    {
        yield return new WaitForSecondsRealtime(2);
        deathWindow.enabled = true;
    }

    public void Restart()
    {
        deathWindow.enabled = false;
        pauseButton.enabled = true;
    }

    public void Pause()
    {
        GameManager.audioManager.PlaySound(tapSound);
        BroadcastMessages<bool>.SendMessage(Messages.PAUSE, true);
    }
    public void IsPause(bool isPause) => pauseWindow.enabled = isPause;
}
