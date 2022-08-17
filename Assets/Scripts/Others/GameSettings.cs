using System;
[Serializable]
public struct GameSettings
{
    public enum Quality
    {
        Low, Medium, High
    }
    public bool sound;
    public bool music;
    public Quality quality;
}
