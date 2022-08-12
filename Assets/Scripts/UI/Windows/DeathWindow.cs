using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class DeathWindow : MonoBehaviour
{
    [SerializeField] AudioClip tapSound;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject mainMenuButton;
    [SerializeField] TextMeshProUGUI health;
    Canvas canvas;

    void OnEnable() => BroadcastMessages.AddListener(Messages.DEATH, Death);
    void OnDisable() => BroadcastMessages.RemoveListener(Messages.DEATH, Death);

    void Death()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        if (encodedData.health < encodedData.revivalCost)
        {
            mainMenuButton.transform.position = restartButton.transform.position;
            restartButton.SetActive(false);
        }
        else
        {
            restartButton.SetActive(true);
            health.text = encodedData.revivalCost.ToString();
        }
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
