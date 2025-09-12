using UnityEngine;
using UnityEngine.Playables;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject objects;
    public PlayableDirector scenes;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OpenCutScene();
    }

    private void OpenCutScene()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            objects.SetActive(true);
            scenes.Play();
        }
    }
}
