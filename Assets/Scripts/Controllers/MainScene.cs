using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneController
{
    void Start()
    {
        Managers.audioManager.PlayMusic(Resources.Load("Music/" + playMusic) as AudioClip);
    }
}
