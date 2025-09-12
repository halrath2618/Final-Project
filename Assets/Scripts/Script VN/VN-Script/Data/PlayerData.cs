using System;

[Serializable]
public class PlayerData
{
    public float storyProcess;
    public PlayerData(float progress)
    {
        storyProcess = progress;

    }
}
