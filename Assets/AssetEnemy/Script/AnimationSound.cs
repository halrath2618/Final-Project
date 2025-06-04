using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    [SerializeField] private AudioSource _local;

    
    public void PlayHitSound(string name)
    {
      
            if (_local != null)
            {
                var clip = AudioManager.Instance.GetHitClip(name);
                if (clip != null) _local.PlayOneShot(clip);
            }
            else
            {
                AudioManager.Instance.PlayHitSound(name);
            }

        
    }
}
