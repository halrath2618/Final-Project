using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; // Singleton ?? qu?n lý nh?c toàn game
    public AudioSource audioSource;    // Ngu?n phát nh?c

    private void Awake()
    {
        // N?u ?ã có BGMManager khác, xóa cái này ?? tránh trùng l?p
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Gi? nh?c t?n t?i khi ??i scene
        DontDestroyOnLoad(gameObject);

        // N?u ch?a có AudioSource thì t? thêm
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // C?u hình c? b?n cho nh?c
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    // Hàm b?t nh?c m?i
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    // Hàm d?ng nh?c
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // Hàm ??i volume
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}
