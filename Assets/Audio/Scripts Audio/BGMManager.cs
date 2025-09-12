using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; // Singleton ?? qu?n l� nh?c to�n game
    public AudioSource audioSource;    // Ngu?n ph�t nh?c

    private void Awake()
    {
        // N?u ?� c� BGMManager kh�c, x�a c�i n�y ?? tr�nh tr�ng l?p
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Gi? nh?c t?n t?i khi ??i scene
        DontDestroyOnLoad(gameObject);

        // N?u ch?a c� AudioSource th� t? th�m
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // C?u h�nh c? b?n cho nh?c
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    // H�m b?t nh?c m?i
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    // H�m d?ng nh?c
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // H�m ??i volume
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}
