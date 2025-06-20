using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Monster monsterHealth;

    [SerializeField] private CinemachineCamera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the camera toward HP Bar
        hpBar.transform.rotation = camera.transform.rotation;
    }
}
