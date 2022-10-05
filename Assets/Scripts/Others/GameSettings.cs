using System;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public struct GameSettings
    {
        public bool sound;
        public bool music;
        public Quality quality;

        public GameSettings(bool _sound, bool _music, Quality _quality)
        {
            sound = _sound;
            music = _music;
            quality = _quality;
        }
    }
    public enum Quality
    {
        Low, Medium, High
    }
}
