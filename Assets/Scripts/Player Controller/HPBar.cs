using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    public Slider hpSlider; // Reference to the Slider component for the HP bar
    public Slider manaSlider; // Reference to the UI Slider for Mana
    public Slider staminaSlider; // Reference to the UI Slider for Stamina

    private PlayerStatsManager playerStatsManager; // Reference to the PlayerStatsManager script
    private PlayerController playerController; // Reference to the PlayerController script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateHP();
        UpdateMana(); // Call the UpdateMana method to update the Mana bar
        UpdateStamina(); // Call the UpdateStamina method to update the Stamina bar
    }

    public void UpdateHP()
    {
        hpSlider.maxValue = playerController.maxHP; // Set the maximum value of the HP bar to the player's max HP
        hpSlider.value = playerStatsManager.health; // Set the current value of the HP bar to the player's current health
    }
    public void UpdateMana()
    {
        manaSlider.maxValue = playerController.maxMana; // Set the maximum value of the Mana bar to the player's max Mana
        manaSlider.value = playerStatsManager.mana; // Set the current value of the Mana bar to the provided manaValue
    }
    public void UpdateStamina()
    {
        staminaSlider.maxValue = playerController.maxStamina; // Set the maximum value of the Stamina bar to the player's max Stamina
        staminaSlider.value = playerStatsManager.stamina; // Set the current value of the Stamina bar to the player's current Stamina
    }
}
