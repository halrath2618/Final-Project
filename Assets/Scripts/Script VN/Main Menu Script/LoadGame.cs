using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public Button YES;
    public Button NO;
    private int loadsavefile = 5;

    public int LoadGameYes()
    {
        loadsavefile = 1;
        SceneManager.LoadScene("VisualNovel");
        return loadsavefile;
    }
    public int LoadGameNo()
    {
        loadsavefile = 0;
        SceneManager.LoadScene("VisualNovel");
        return loadsavefile;
    }

    private void Start()
    {

    }
}

