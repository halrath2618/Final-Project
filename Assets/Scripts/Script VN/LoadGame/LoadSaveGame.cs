using UnityEngine;
using UnityEngine.UI;

public class LoadSaveGame : MonoBehaviour
{
    public Button YES;
    public Button NO;
    private int loadsavefile = 0;

    public void LoadGameYes()
    {
        loadsavefile++;
    }
    public void LoadGameNo()
    {
        loadsavefile += 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
