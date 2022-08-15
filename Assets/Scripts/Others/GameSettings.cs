using System;
[Serializable]
public struct GameSettings
{
    public enum Quality
    {
        Low, Medium, High
    }
    public bool soundMute;
    public bool musicMute;
    public Quality quality;
}
