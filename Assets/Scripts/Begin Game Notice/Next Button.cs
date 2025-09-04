using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour
{
    public void NextSceneButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
