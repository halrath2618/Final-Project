using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    public Slider hpSlider; // Reference to the Slider component for the HP bar


    public PlayerController playerController; // Reference to the PlayerController script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HP();
    }

    public void HP()
    {
        hpSlider.maxValue = playerController.maxHP; // Set the maximum value of the HP bar to the player's max HP
        hpSlider.value = playerController._currentHealth; // Set the current value of the HP bar to the player's current health
    }
}
