using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IManagers
{
    [SerializeField] Canvas mainSceneUI;
    [SerializeField] Canvas playSceneUI;
    [SerializeField] Canvas settingsWindow;
    [SerializeField] Canvas loadWindow;
    [SerializeField] TextMeshProUGUI loadText;
    [SerializeField] Slider loadBar;
    [SerializeField] AudioClip tapSound;
    int buildIndex;

    public void StartManager()
    {
        List<Canvas> UIs = new List<Canvas>();
        UIs.Add(mainSceneUI);
        UIs.Add(playSceneUI);
        UIs.Add(loadWindow);
        UIs.Add(settingsWindow);

        foreach (Canvas ui in UIs)
        {
            ui.gameObject.GetComponent<IUserInterface>()?.StartUI();
            ui.enabled = false;
        }
    }

    public void ActiveUI(int index)
    {
        buildIndex = index;
        loadWindow.enabled = false;
        mainSceneUI.enabled = buildIndex is 1;
        playSceneUI.enabled = buildIndex > 1;
    }

    public void OpenSettings(bool isOpen)
    {
        GameManager.audioManager.PlaySound(tapSound);
        settingsWindow.enabled = isOpen;
    }

    public void LoadScene(float progress)
    {
        loadWindow.enabled = true;
        mainSceneUI.enabled = false;
        playSceneUI.enabled = false;
        loadBar.value = progress;
        float percentages = progress * 100f;
        loadText.text = "Loading: " + (int)percentages + "%";
    }
}
