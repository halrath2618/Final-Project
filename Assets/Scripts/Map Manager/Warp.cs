using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the warp area.");
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
