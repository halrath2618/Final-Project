using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{

    [Header("Skill Cooldown Settings")]
    public Slider skillCooldownSliderE; // Reference to the UI Slider for cooldown
    public Slider skillCooldownSliderF; // Reference to the UI Slider for cooldown of the second skill
    public PlayerController playerController; // Reference to the PlayerController script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        E_UpdateSkillCooldown(); // Call the UpdateSkillCooldown method to update the cooldown slider
        F_UpdateSkillCooldown(); // Call the UpdateSkillCooldown method to update the cooldown slider for the second skill
    }
    public void E_UpdateSkillCooldown()
    {
        skillCooldownSliderE.maxValue = playerController.e_maxSkillCooldown; // Set the maximum value of the cooldown slider to the spell's cooldown time
        skillCooldownSliderE.value = playerController.e_cdTime; // Set the current value of the cooldown slider to the spell's cooldown time
    }
    public void F_UpdateSkillCooldown()
    {
        skillCooldownSliderF.maxValue = playerController.f_maxSkillCooldown; // Set the maximum value of the cooldown slider to the spell's cooldown time
        skillCooldownSliderF.value = playerController.f_cdTime; // Set the current value of the cooldown slider to the spell's cooldown time
    }
}
