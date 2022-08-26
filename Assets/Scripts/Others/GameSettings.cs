using System;

namespace Settings
{
    [Serializable]
    public struct GameSettings
    {
        public bool sound;
        public bool music;
        public Quality quality;
    }
    public enum Quality
    {
        Low, Medium, High
    }
}
