using UnityEngine;
using System.Collections;
using TMPro;

public class SkillCoolDownManager : MonoBehaviour
{
    [Header("Cooldown Settings")]
    public float skill1MaxCD; // Cooldown time for skills in seconds
    public float skill1CDTime;
    public bool skill1_isReady = true; // Flag to check if the skill is ready to be used
    public SkillCooldown skillCooldown;
    public GameObject skill1_cdSlider;
    public GameObject skill2_cdSlider;
    public float skill2MaxCD; // Cooldown time for skills in seconds
    public float skill2CDTime;
    public bool skill2_isReady = true; // Flag to check if the skill is ready to be used
    public Warning_Skill warningSkill;
    public SkillConstantlyActive skillConstantlyActive;
    public bool auraReady; // Flag to check if the aura skill is ready to be used
    public GameObject HP_Potion;
    public GameObject Mana_Potion;
    public float hpMaxCD = 10f;
    public float hpDuration = 10f;
    public float manaDuration = 10f;
    public float manaMaxCD = 10f;
    public float hpcdTime = 10f;
    public float manacdTime = 10f;
    public bool hpcdReady = true; // Flag to check if the HP potion cooldown is ready
    public bool manacdReady = true; // Flag to check if the Mana potion cooldown is ready
    private PlayerStatsManager playerStatsManager;
    public TMP_Text HPCount;
    public TMP_Text MPCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
        warningSkill = GetComponent<Warning_Skill>();
        skillConstantlyActive = GetComponent<SkillConstantlyActive>();
        skillCooldown = GetComponent<SkillCooldown>();
        auraReady = true;
    }
    private void Update()
    {
        HPCount.text = playerStatsManager.HPPotionCount.ToString();
        MPCount.text = playerStatsManager.MPPotionCount.ToString();
    }
    // Update is called once per frame
    public IEnumerator ApplySkillCooldown1()
    {
        while (!skill1_isReady)
        {
            skill1_cdSlider.SetActive(true);
            yield return skill1CDTime -= Time.deltaTime; // Decrease cooldown time
            if (skill1CDTime <= 0)
            {
                skill1CDTime = 0; // Ensure cooldown does not go below zero
                skill1_isReady = true; // Skill is ready again
                skill1_cdSlider.SetActive(false); // Hide cooldown slider
                break;
            }
        }
        skill1CDTime = skill1MaxCD; // Reset cooldown time
        skillCooldown.E_UpdateSkillCooldown(); // Update the cooldown slider UI
    }
    public IEnumerator ApplySkillCooldown2()
    {
        while (!skill2_isReady)
        {
            skill2_cdSlider.SetActive(true);
            yield return skill2CDTime -= Time.deltaTime; // Decrease cooldown time
            if (skill2CDTime <= 0)
            {
                skill2CDTime = 0; // Ensure cooldown does not go below zero
                skill2_isReady = true; // Skill is ready again
                skill2_cdSlider.SetActive(false); // Hide cooldown slider
                break;
            }
        }
        skill2CDTime = skill2MaxCD; // Reset cooldown time
        skillCooldown.F_UpdateSkillCooldown(); // Update the cooldown slider UI
    }
    public IEnumerator ApplySkillCooldownHPPotion()
    {
        while (hpcdTime > 0)
        {
            HP_Potion.SetActive(true);
            yield return hpcdTime -= Time.deltaTime; // Decrease cooldown time
            if (hpcdTime <= 0)
            {
                hpcdTime = 0; // Ensure cooldown does not go below zero
                hpcdReady = true; // Skill is ready again
                HP_Potion.SetActive(false); // Hide cooldown slider
                break;
            }
        }
        hpcdTime = hpMaxCD; // Reset cooldown time
        skillCooldown.HPCooldownUpdate(); // Update the cooldown slider UI
    }

    public IEnumerator ApplySkillCooldownManaPotion()
    {
        while (manacdTime > 0)
        {
            Mana_Potion.SetActive(true);
            yield return manacdTime -= Time.deltaTime; // Decrease cooldown time
            if (manacdTime <= 0)
            {
                manacdTime = 0; // Ensure cooldown does not go below zero
                manacdReady = true; // Skill is ready again
                Mana_Potion.SetActive(false); // Hide cooldown slider
                break;
            }
        }
        manacdTime = manaMaxCD; // Reset cooldown time
        skillCooldown.MPCooldownUpdate(); // Update the cooldown slider UI
    }
}
