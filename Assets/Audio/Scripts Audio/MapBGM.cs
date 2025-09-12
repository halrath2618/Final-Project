using UnityEngine;

public class MapBGM : MonoBehaviour
{
    [Header("Chon nhac nen cho map nay")]
    public AudioClip mapMusic;

    void Start()
    {
        if (BGMManager.Instance != null && mapMusic != null)
        {
            BGMManager.Instance.PlayMusic(mapMusic);
        }
    }
}
