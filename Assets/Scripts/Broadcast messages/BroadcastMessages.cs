using System.Collections.Generic;
using System;

///<summary>
///Класс для рассылки сообщений всем подписчикам о каком-либо событии в игре.
///Используется, если необходимо вызвать сразу несколько методов, к которым нет прямого доступа.
///Для этого методы обязательно должны быть подписаны на определённое событие.
///Может типизироваться максимум тремя универсальными параметрами
///</summary>
public static class BroadcastMessages
{
    readonly public static Dictionary<MessageType, Message> dict = new Dictionary<MessageType, Message>();

    ///<summary>
    ///Подписывает метод на определённое событие. 
    ///Подписанный метод обязательно должен быть затем отписан от рассылки
    ///</summary>
    ///<param name="message">Тип события (сообщения)</param>
    ///<param name="listener">Подписчик (метод) на событие</param>
    public static void AddListener(MessageType message, Action listener)
    {
        if (!dict.ContainsKey(message))
            dict.Add(message, new Message());
        dict[message].AddListener(listener);
    }
    ///<summary>
    ///Отписывает метод от рассылки сообщения
    ///</summary>
    ///<param name="message">Тип события (сообщения)</param>
    ///<param name="listener">Подписчик (метод) на событие</param>
    public static void RemoveListener(MessageType message, Action listener)
    {
        if (dict.ContainsKey(message))
            dict[message].RemoveListener(listener);
        if (dict[message].listeners.Count == 0)
            dict.Remove(message);
    }
    ///<summary>
    ///Рассылает сообщение всем подписчикам
    ///</summary>
    ///<param name="message">Сообщение для рассылки</param>
    public static void SendMessage(MessageType message)
    {
        if (dict.ContainsKey(message))
            dict[message].SendMessage();
    }
}

///<inheritdoc cref="BroadcastMessages"/>
public static class BroadcastMessages<T1>
{
    readonly public static Dictionary<MessageType, Message<T1>> dict = new Dictionary<MessageType, Message<T1>>();

    ///<inheritdoc cref="BroadcastMessages.AddListener"/>
    public static void AddListener(MessageType message, Action<T1> listener)
    {
        if (!dict.ContainsKey(message))
            dict.Add(message, new Message<T1>());
        dict[message].AddListener(listener);
    }
    ///<inheritdoc cref="BroadcastMessages.RemoveListener"/>
    public static void RemoveListener(MessageType message, Action<T1> listener)
    {
        if (dict.ContainsKey(message))
            dict[message].RemoveListener(listener);
        if (dict[message].listeners.Count == 0)
            dict.Remove(message);
    }
    ///<inheritdoc cref="BroadcastMessages.SendMessage"/>
    public static void SendMessage(MessageType message, T1 a)
    {
        if (dict.ContainsKey(message))
            dict[message].SendMessage(a);
    }
}

///<inheritdoc cref="BroadcastMessages"/>
public static class BroadcastMessages<T1, T2>
{
    readonly public static Dictionary<MessageType, Message<T1, T2>> dict = new Dictionary<MessageType, Message<T1, T2>>();

    ///<inheritdoc cref="BroadcastMessages.AddListener"/>
    public static void AddListener(MessageType message, Action<T1, T2> listener)
    {
        if (!dict.ContainsKey(message))
            dict.Add(message, new Message<T1, T2>());
        dict[message].AddListener(listener);
    }
    ///<inheritdoc cref="BroadcastMessages.RemoveListener"/>
    public static void RemoveListener(MessageType message, Action<T1, T2> listener)
    {
        if (dict.ContainsKey(message))
            dict[message].RemoveListener(listener);
        if (dict[message].listeners.Count == 0)
            dict.Remove(message);
    }
    ///<inheritdoc cref="BroadcastMessages.SendMessage"/>
    public static void SendMessage(MessageType message, T1 a, T2 b)
    {
        if (dict.ContainsKey(message))
            dict[message].SendMessage(a, b);
    }
}

///<inheritdoc cref="BroadcastMessages"/>
public static class BroadcastMessages<T1, T2, T3>
{
    readonly public static Dictionary<MessageType, Message<T1, T2, T3>> dict = new Dictionary<MessageType, Message<T1, T2, T3>>();

    ///<inheritdoc cref="BroadcastMessages.AddListener"/>
    public static void AddListener(MessageType message, Action<T1, T2, T3> listener)
    {
        if (!dict.ContainsKey(message))
            dict.Add(message, new Message<T1, T2, T3>());
        dict[message].AddListener(listener);
    }
    ///<inheritdoc cref="BroadcastMessages.RemoveListener"/>
    public static void RemoveListener(MessageType message, Action<T1, T2, T3> listener)
    {
        if (dict.ContainsKey(message))
            dict[message].RemoveListener(listener);
        if (dict[message].listeners.Count == 0)
            dict.Remove(message);
    }
    ///<inheritdoc cref="BroadcastMessages.SendMessage"/>
    public static void SendMessage(MessageType message, T1 a, T2 b, T3 c)
    {
        if (dict.ContainsKey(message))
            dict[message].SendMessage(a, b, c);
    }
}

