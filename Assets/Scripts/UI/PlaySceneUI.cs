using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Security;

///<summary>
///Класс, управляющий пользовательским интерфейсом в игровой сцене
///</summary>
public class PlaySceneUI : MonoBehaviour
{
    [SerializeField] Canvas deathWindow;
    [SerializeField] Button pauseButton;
    [SerializeField] private TextMeshProUGUI score; // Набранные очки
    [SerializeField] private TextMeshProUGUI moneys; // Собранные монеты
    [SerializeField] private TextMeshProUGUI multiplier; // Множитель очков

    void OnEnable()
    {
        BroadcastMessages.AddListener(MessageType.DEATH, Death);
        BroadcastMessages.AddListener(MessageType.RESTART, Restart);
        BroadcastMessages<bool>.AddListener(MessageType.PAUSE, IsPause);
    }
    void OnDisable()
    {
        BroadcastMessages.RemoveListener(MessageType.DEATH, Death);
        BroadcastMessages.RemoveListener(MessageType.RESTART, Restart);
        BroadcastMessages<bool>.RemoveListener(MessageType.PAUSE, IsPause);
    }

    void Update()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        int scoreText = (int)encodedData.score;
        moneys.text = Managers.uiManager.AddSeparator(encodedData.money.ToString());
        score.text = Managers.uiManager.AddSeparator(scoreText.ToString());
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
        Managers.uiManager.OpenPopupWindow(deathWindow);
    }

    public void Pause()
    {
        BroadcastMessages<bool>.SendMessage(MessageType.PAUSE, true);
    }
    public void IsPause(bool isPause)
    {
        pauseButton.interactable = !isPause;
    }
}
