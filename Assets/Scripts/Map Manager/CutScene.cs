using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    public PlayableDirector cutScene; // Reference to the PlayableDirector component for the cutscene
    public GameObject cutSceneObject; // Reference to the GameObject that contains the cutscene
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutSceneObject.SetActive(true); // Activate the cutscene GameObject
            cutScene.Play();
            StartCoroutine(WaitForCutsceneEnd());
        }
    }
    IEnumerator WaitForCutsceneEnd()
    {
        // Wait until the cutscene is done playing
        while (cutScene.state == PlayState.Playing)
        {
            yield return null; // Wait for the next frame
        }

        // Load the next scene after the cutscene ends
        SceneManager.LoadScene("LoadingScene 1");
    }
}
