using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the warp area.");
            SceneManager.LoadScene("Map 1"); // Load the specified scene when the player enters the warp area
        }
    }
}
