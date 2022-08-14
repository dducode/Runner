public struct GameSettings
{
    public bool soundMute;
    public bool musicMute;

    GameSettings(bool _soundMute = false, bool _musicMute = true)
    {
        soundMute = _soundMute;
        musicMute = _musicMute;
    }
}
