using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource sfxSource;

    public AudioClip[] soundtracks;
    public AudioClip[] soundEffects;

    public string[] soundtrackNames;
    public string[] sfxNames;
    void Start()
    {
        if (soundtracks.Length > 0)
        {
            PlayTrackByName("ForestNight"); // Default track name
        }
    }
    public void PlayTrackByName(string name)
    {
        for (int i = 0; i < soundtrackNames.Length; i++)
        {
            if (soundtrackNames[i] == name)
            {
                audioSource.clip = soundtracks[i];
                audioSource.Play();
                return;
            }
        }
        Debug.LogWarning("Soundtrack not found: " + name);
    }
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
