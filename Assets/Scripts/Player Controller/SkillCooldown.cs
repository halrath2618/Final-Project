using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{

    [Header("Skill Cooldown Settings")]
    public Slider skillCooldownSliderE; // Reference to the UI Slider for cooldown
    public Slider skillCooldownSliderF; // Reference to the UI Slider for cooldown of the second skill
    public Slider HP;
    public Slider MP;
    private SkillCoolDownManager skillCoolDownManager; // Reference to the PlayerController script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillCoolDownManager = GetComponent<SkillCoolDownManager>(); // Get the PlayerController component from the same GameObject
    }

    // Update is called once per frame
    void Update()
    {
        E_UpdateSkillCooldown(); // Call the UpdateSkillCooldown method to update the cooldown slider
        F_UpdateSkillCooldown(); // Call the UpdateSkillCooldown method to update the cooldown slider for the second skill
        HPCooldownUpdate(); // Call the Update method to update the HP slider
        MPCooldownUpdate(); // Call the Update method to update the MP slider
    }
    public void E_UpdateSkillCooldown()
    {
        skillCooldownSliderE.maxValue = skillCoolDownManager.skill1MaxCD; // Set the maximum value of the cooldown slider to the spell's cooldown time
        skillCooldownSliderE.value = skillCoolDownManager.skill1CDTime; // Set the current value of the cooldown slider to the spell's cooldown time
    }
    public void F_UpdateSkillCooldown()
    {
        skillCooldownSliderF.maxValue = skillCoolDownManager.skill2MaxCD; // Set the maximum value of the cooldown slider to the spell's cooldown time
        skillCooldownSliderF.value = skillCoolDownManager.skill2CDTime; // Set the current value of the cooldown slider to the spell's cooldown time
    }
    public void HPCooldownUpdate()
    {
        HP.maxValue = skillCoolDownManager.hpMaxCD; // Set the maximum value of the HP slider to the player's maximum HP
        HP.value = skillCoolDownManager.hpcdTime; // Set the current value of the HP slider to the player's current HP
    }
    public void MPCooldownUpdate()
    {
        MP.maxValue = skillCoolDownManager.manaMaxCD; // Set the maximum value of the MP slider to the player's maximum MP
        MP.value = skillCoolDownManager.manacdTime; // Set the current value of the MP slider to the player's current MP
    }
}
