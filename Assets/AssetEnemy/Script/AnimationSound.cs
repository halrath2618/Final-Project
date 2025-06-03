using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    [SerializeField] private AudioSource _local;
    [SerializeField] private Transform _player;
    [SerializeField] private float maxDistance = 10f ;

    private void Start()
    {
        if (_local != null)
        {
            _local.spatialBlend = 1f; // Bật chế độ 3D
            _local.minDistance = 2f;  // Nghe rõ trong khoảng này
            _local.maxDistance = 10f; // Âm thanh mờ dần sau khoảng này
            _local.rolloffMode = AudioRolloffMode.Linear; // Kiểu giảm âm
        }
    }
    public void PlayHitSound(string name)
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            if(_player == null)  return;
        }

        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance <= maxDistance)
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
}
