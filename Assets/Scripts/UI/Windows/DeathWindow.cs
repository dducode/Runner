using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Security;

[RequireComponent(typeof(Canvas))]
public class DeathWindow : MonoBehaviour
{
    [SerializeField] AudioClip tapSound;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI health;
    Canvas canvas;
    Vector3 startPos;

    void OnEnable() => BroadcastMessages.AddListener(MessageType.DEATH, Death);
    void OnDisable() => BroadcastMessages.RemoveListener(MessageType.DEATH, Death);

    void Death()
    {
        EncodedData encodedData = Managers.dataManager.GetData();
        restartButton.interactable = encodedData.health > encodedData.revivalCost;
        health.text = encodedData.revivalCost.ToString();
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Restart()
    {
        Managers.audioManager.PlaySound(tapSound);
        canvas.enabled = false;
        BroadcastMessages.SendMessage(MessageType.RESTART);
    }

    public void MainMenu()
    {
        Managers.audioManager.PlaySound(tapSound);
        canvas.enabled = false;
        Managers.gameManager.LoadScene(1);
    }
}
