using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TriggerCutscene : MonoBehaviour
{
    public VideoPlayer player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isPaused)
        {
            SceneManager.LoadScene("Prologue");
        }
    }
}
