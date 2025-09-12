using UnityEngine;

public class AudioSFXManager : MonoBehaviour
{
    public AudioClip[] soundtracks;
    public AudioClip[] soundEffects;

    public AudioSource sfxSource;

    public string[] sfxNames;

    public void PlaySFXByName(string name)
    {
        for (int i = 0; i < sfxNames.Length; i++)
        {
            if (sfxNames[i] == name)
            {
                sfxSource.PlayOneShot(soundEffects[i]);
                return;
            }
        }
        Debug.LogWarning("Sound effect not found: " + name);
    }
}
