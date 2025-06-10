using UnityEngine;
using UnityEngine.UI;

public class ManaStamina : MonoBehaviour
{
    
    public PlayerController playerController; // Reference to the PlayerController script
    public Slider manaSlider; // Reference to the UI Slider for Mana
    public Slider staminaSlider; // Reference to the UI Slider for Stamina
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMana(); // Call the UpdateMana method to update the Mana bar
        UpdateStamina(); // Call the UpdateStamina method to update the Stamina bar
    }
    public void UpdateMana()
    {
        manaSlider.maxValue = playerController.maxMana; // Set the maximum value of the Mana bar to the player's max Mana
        manaSlider.value = playerController._currentMana; // Set the current value of the Mana bar to the provided manaValue
    }
    public void UpdateStamina()
    {
        staminaSlider.maxValue = playerController.maxStamina; // Set the maximum value of the Stamina bar to the player's max Stamina
        staminaSlider.value = playerController._currentStamina; // Set the current value of the Stamina bar to the player's current Stamina
    }
}
