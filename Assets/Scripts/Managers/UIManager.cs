using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

///<summary>
///Менеджер игрового пользовательского интерфейса
///</summary>
public class UIManager : MonoBehaviour, IManagers
{
    [SerializeField] Canvas mainSceneUI;
    [SerializeField] Canvas playSceneUI;
    [SerializeField] Canvas settingsWindow;
    [SerializeField] Canvas loadWindow;
    [SerializeField] TextMeshProUGUI loadText;
    [SerializeField] Slider loadBar;
    [SerializeField] AudioClip tapSound;
    Stack<Canvas> windows;

    public void StartManager()
    {
        windows = new Stack<Canvas>();
        Canvas[] UIs = GetComponentsInChildren<Canvas>();
        foreach (Canvas ui in UIs)
        {
            ui.GetComponent<IUserInterface>()?.StartUI();
            ui.enabled = false;
        }
    }

    ///<summary>
    ///Обновляет отображение UI
    ///</summary>
    public void UpdateViews() => mainSceneUI.GetComponent<MainSceneUI>().UpdateViews();

    ///<summary>
    ///Добавляет разделитель между числовыми классами
    ///</summary>
    public string AddSeparator(string target, char separator = ' ')
    {
        List<char> symbols = new List<char>();
        symbols.AddRange(target);
        int initialCount = symbols.Count;
        for (int i = initialCount - 3; i > 0; i -= 3)
            symbols.Insert(i, separator);
        return String.Join<char>(null, symbols);
    }

    ///<summary>
    ///Используется для открытия окна пользователем из UI
    ///</summary>
    public void OpenWindow(Canvas window)
    {
        Managers.audioManager.PlaySound(tapSound);
        Canvas w = null;
        windows.TryPeek(out w);
        if (w != null) w.enabled = false;

        windows.Push(window);
        window.enabled = true;
    }
    ///<summary>
    ///Используется для открытия всплывающего окна из скрипта
    ///</summary>
    public void OpenPopupWindow(Canvas window)
    {
        Canvas w = null;
        windows.TryPeek(out w);
        if (w != null) w.enabled = false;

        windows.Push(window);
        window.enabled = true;
    }
    ///<summary>
    ///Используется для закрытия текущего окна
    ///</summary>
    public void CloseWindow()
    {
        Managers.audioManager.PlaySound(tapSound);
        Canvas window = windows.Pop();
        window.enabled = false;

        Canvas w = null;
        windows.TryPeek(out w);
        if (w != null) w.enabled = true;
    }

    public void StartLoad()
    {
        loadWindow.enabled = true;
        mainSceneUI.enabled = false;
        playSceneUI.enabled = false;
    }
    public void LoadScene(float progress)
    {
        loadBar.value = progress;
        float percentages = progress * 100f;
        loadText.text = "Loading: " + (int)percentages + "%";
    }
    public void LoadCompleted(int sceneIndex)
    {
        loadWindow.enabled = false;
        mainSceneUI.enabled = sceneIndex is 1;
        playSceneUI.enabled = sceneIndex > 1;
    }
}
