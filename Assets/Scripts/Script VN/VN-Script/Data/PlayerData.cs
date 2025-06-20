using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable] public class PlayerData
{
    public float storyProcess;
    public PlayerData(float progress)
    {
        storyProcess = progress;

    }
}
