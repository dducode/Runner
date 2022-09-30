using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

///<summary>
///Основной игровой менеджер
///</summary>
public class GameManager : MonoBehaviour, IManagers
{
    AsyncOperation async;
    Scene scene;

    public void StartManager()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        LoadScene(1);
    }

    void OnEnable() => BroadcastMessages<bool>.AddListener(MessageType.PAUSE, OnPause);
    void OnDisable() => BroadcastMessages<bool>.RemoveListener(MessageType.PAUSE, OnPause);

    ///<summary>
    ///Загружает сцену по индексу
    ///</summary>
    ///<param name="scene">Индекс сцены в сборке</param>
    public void LoadScene(int scene)
    {
        async = SceneManager.LoadSceneAsync(scene);
        StartCoroutine(LoadProgress());
        async.completed += LoadCompleted;
    }
    void LoadCompleted(AsyncOperation _async)
    {
        BroadcastMessages<bool>.SendMessage(MessageType.PAUSE, false);
        scene = SceneManager.GetActiveScene();
        Managers.uiManager.ActiveUI(scene.buildIndex);
        Managers.uiManager.UpdateViews();
        async.completed -= LoadCompleted;
    }
    IEnumerator LoadProgress()
    {
        while (!async.isDone)
        {
            Managers.uiManager.LoadScene(async.progress);
            yield return null;
        }
    }
    public void OnPause(bool isPause) => Time.timeScale = isPause ? 0f : 1f;
    public void OnApplicationPause(bool pause) => BroadcastMessages<bool>.SendMessage(MessageType.PAUSE, pause);
}
