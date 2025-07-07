using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Health monsterHealth;

    [SerializeField] private CinemachineCamera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        //rotate the camera toward HP Bar
        if (hpBar != null)
        {
            hpBar.transform.rotation = camera.transform.rotation;
        }
    }

    public void UpdateHealthBar()
    {
        if (hpBar != null)
        {
            hpBar.maxValue = monsterHealth.maxHealth; // Set the maximum value of the HP bar to the monster's max health
            hpBar.value = monsterHealth.currentHealth; // Set the current value of the HP bar to the monster's current health
        }
    }
}
