using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    Canvas otherWindow;

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

    public void UpdateViews() => mainSceneUI.GetComponent<MainSceneUI>().UpdateViews();

    public string StringConversion(string target)
    {
        List<char> symbols = new List<char>();
        symbols.AddRange(target);
        int initialCount = symbols.Count;
        for (int i = initialCount - 3; i > 0; i -= 3)
            symbols.Insert(i, ' ');
        return String.Join<char>(null, symbols);
    }

    public void ActiveUI(int index)
    {
        buildIndex = index;
        loadWindow.enabled = false;
        mainSceneUI.enabled = buildIndex is 1;
        playSceneUI.enabled = buildIndex > 1;
    }

    public void OpenSettings(Canvas _otherWindow)
    {
        otherWindow = _otherWindow;
        OpenSettings(true);
    }

    public void OpenSettings(bool isOpen)
    {
        GameManager.audioManager.PlaySound(tapSound);
        settingsWindow.enabled = isOpen;
        otherWindow.enabled = !isOpen;
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
