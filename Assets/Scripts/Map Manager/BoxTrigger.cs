using Runemark.Common.Gameplay;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    private RMRotator rotator;
    [SerializeField] private GameObject fx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rotator = GetComponent<RMRotator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fx.SetActive(true);
            rotator.Activate();
        }
    }
}
