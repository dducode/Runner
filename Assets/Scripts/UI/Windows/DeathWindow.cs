using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class DeathWindow : MonoBehaviour
{
    [SerializeField] AudioClip tapSound;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI health;
    Canvas canvas;
    Vector3 startPos;

    void OnEnable() => BroadcastMessages.AddListener(Messages.DEATH, Death);
    void OnDisable() => BroadcastMessages.RemoveListener(Messages.DEATH, Death);

    void Death()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        restartButton.interactable = encodedData.health > encodedData.revivalCost;
        health.text = encodedData.revivalCost.ToString();
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Restart()
    {
        GameManager.audioManager.PlaySound(tapSound);
        canvas.enabled = false;
        BroadcastMessages.SendMessage(Messages.RESTART);
    }

    public void MainMenu()
    {
        GameManager.audioManager.PlaySound(tapSound);
        canvas.enabled = false;
        GameManager.gameManager.LoadScene(1);
    }
}
