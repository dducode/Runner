using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
    event Action eventTimer;
    IEnumerator coroutine;
    /// <summary>
    /// Укажите метод и количество времени (в сек.), через которое должен выполниться данный метод
    /// </summary>
    public void AddListener(float time, Action listener)
    {
        if (coroutine is not null)
            StopCoroutine(coroutine);
        eventTimer = listener;
        coroutine = TimerCounter(time);
        StartCoroutine(coroutine);
    }
    /// <summary>
    /// Запустите несколько методов через заданное количество времени (в сек.)
    /// </summary>
    public void AddListeners(float time, List<Action> listeners)
    {
        if (coroutine is not null)
            StopCoroutine(coroutine);
        foreach (Action listener in listeners)
            eventTimer += listener;
        coroutine = TimerCounter(time);
        StartCoroutine(coroutine);
    }
    IEnumerator TimerCounter(float time)
    {
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        eventTimer?.Invoke();
        eventTimer = null;
    }
}
