using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    public PlayableDirector cutScene; // Reference to the PlayableDirector component for the cutscene
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the cutscene when the player enters the trigger
            cutScene.Play();
            // Optionally, you can disable the player controls or other game elements here
            // For example, if you have a PlayerController script, you might do:
            // other.GetComponent<PlayerController>().enabled = false;
            // Load the next scene after the cutscene finishes
            cutScene.stopped += OnCutSceneStopped;
        }
    }

    private void OnCutSceneStopped(PlayableDirector director)
    {
        // Load the next scene after the cutscene finishes
        SceneManager.LoadScene("Prologue");
        // Optionally, you can re-enable player controls or other game elements here
        // For example, if you have a PlayerController script, you might do:
        // GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
    }
}
