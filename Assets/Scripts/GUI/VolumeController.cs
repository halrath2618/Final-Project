using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{

    [SerializeField] private Scrollbar masterVolume;
    [SerializeField] private Scrollbar musicVolume;
    [SerializeField] private Scrollbar sfxVolume;

    private AudioSource masterAudioSource;
    private AudioSource musicAudioSource;
    private AudioSource sfxAudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        masterVolume.value = masterAudioSource.volume;
        musicVolume.value = musicAudioSource.volume;
        sfxVolume.value = sfxAudioSource.volume;
    }
}
