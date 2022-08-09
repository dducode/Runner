using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathWindow : MonoBehaviour
{
    [SerializeField] AudioClip tapSound;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject mainMenuButton;
    [SerializeField] TextMeshProUGUI health;

    void OnEnable()
    {
        EncodedData encodedData = GameManager.dataManager.GetGameData();
        if (encodedData.health < encodedData.revivalCost)
        {
            mainMenuButton.transform.position = restartButton.transform.position;
            restartButton.SetActive(false);
        }
        else
            health.text = encodedData.revivalCost.ToString();
    }

    public void Restart()
    {
        GameManager.audioManager.PlaySound(tapSound);
        gameObject.SetActive(false);
        BroadcastMessages.SendMessage(Messages.RESTART);
    }

    public void MainMenu()
    {
        GameManager.audioManager.PlaySound(tapSound);
        gameObject.SetActive(false);
        GameManager.gameManager.LoadScene(1);
    }
}
