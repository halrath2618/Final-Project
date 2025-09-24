using UnityEngine;
using UnityEngine.SceneManagement;

public class WarptoHalrath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Warping to Halrath...");
            SceneManager.LoadScene("Map 1.75");
        }
    }
}
