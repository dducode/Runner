using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Security;

[RequireComponent(typeof(Canvas))]
public class DeathWindow : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI health;

    void OnEnable() => BroadcastMessages.AddListener(MessageType.DEATH, Death);
    void OnDisable() => BroadcastMessages.RemoveListener(MessageType.DEATH, Death);

    void Death()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        restartButton.interactable = encodedData.health >= encodedData.revivalCost;
        health.text = encodedData.revivalCost.ToString();
    }

    public void Restart()
    {
        BroadcastMessages.SendMessage(MessageType.RESTART);
    }

    public void MainMenu()
    {
        Managers.gameManager.LoadScene(1);
    }
}
