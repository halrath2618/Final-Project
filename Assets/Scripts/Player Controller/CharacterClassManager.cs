using UnityEngine;

public class CharacterClassManager : MonoBehaviour
{
    [Header("Player References")]
    private PlayerStatsManager playerStatsManager;
    private PlayerController playerController;
    private SkillCoolDownManager skillCDManager;
    [Header("Class References")]
    [SerializeField] private CharacterClassData[] classDatas;

    [Header("Components")]
    private Animator characterAnimator;
    //[SerializeField] private Transform equipmentSocket;

    private CharacterClass currentClass;
    private void Start()
    {
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
        playerController = GetComponent<PlayerController>();
        skillCDManager = GetComponent<SkillCoolDownManager>();
        characterAnimator = GetComponentInChildren<Animator>();
        // Initialize to Starter class
        SwitchClass(CharacterClass.Starter);
    }
    private void Update()
    {
        if (playerStatsManager.characterClassNum == 2)
        {
            skillCDManager.skill1MaxCD = 1f; // Set cooldown time for Brawler's first skill
            skillCDManager.skill1CDTime = skillCDManager.skill1MaxCD; // Initialize cooldown time for Brawler's first skill
            skillCDManager.skill2MaxCD = 4f; // Set cooldown time for Brawler's second skill
            skillCDManager.skill2CDTime = skillCDManager.skill2MaxCD; // Initialize cooldown time for Brawler's second skill
            playerController.SwitchToBrawler(); // Switch to Brawler class when 1 is pressed
        }
        else if (playerStatsManager.characterClassNum == 3)
        {
            skillCDManager.skill1MaxCD = 1f; // Set cooldown time for Mage's first skill
            skillCDManager.skill1CDTime = skillCDManager.skill1MaxCD; // Initialize cooldown time for Mage's first skill
            skillCDManager.skill2MaxCD = 4f; // Set cooldown time for Mage's second skill
            skillCDManager.skill2CDTime = skillCDManager.skill2MaxCD; // Initialize cooldown time for Mage's second skill
            playerController.SwitchToMage(); // Switch to Mage class when 2 is pressed
        }
        else if (playerStatsManager.characterClassNum == 4)
        {
            skillCDManager.skill1MaxCD = 1f; // Set cooldown time for SwordMaster's first skill
            skillCDManager.skill1CDTime = skillCDManager.skill1MaxCD; // Initialize cooldown time for SwordMaster's first skill
            skillCDManager.skill2MaxCD = 4f; // Set cooldown time for SwordMaster's second skill
            skillCDManager.skill2CDTime = skillCDManager.skill2MaxCD; // Initialize cooldown time for SwordMaster's second skill
            playerController.SwitchToSwordMaster(); // Switch to SwordMaster class when 3 is pressed
        }
        else if (playerStatsManager.characterClassNum == 1)
        {
            skillCDManager.skill1MaxCD = 1f; // Set cooldown time for After Meet Halrath's first skill
            skillCDManager.skill1CDTime = skillCDManager.skill1MaxCD; // Initialize cooldown time for After Meet Halrath's first skill
            skillCDManager.skill2MaxCD = 2f; // Set cooldown time for After Meet Halrath's second skill
            skillCDManager.skill2CDTime = skillCDManager.skill2MaxCD; // Initialize cooldown time for After Meet Halrath's second skill
            playerController.SwitchToMeetHalrath(); // Switch to After Meet Halrath class when 4 is pressed
        }
        else if (playerStatsManager.characterClassNum == 0)
        {
            SwitchClass(CharacterClass.Starter);
        }
    }
    public void SwitchClass(CharacterClass newClass)
    {
        if (currentClass == newClass) return;

        // Find class data
        CharacterClassData classData = GetClassData(newClass);
        if (classData == null)
        {
            Debug.LogError($"No data found for class: {newClass}");
            return;
        }

        // Switch animator controller
        characterAnimator.runtimeAnimatorController = classData.animatorController;

        // Set avatar if using humanoid rig
        if (classData.avatar != null)
        {
            characterAnimator.avatar = classData.avatar;
        }

        // Update state
        currentClass = newClass;
        Debug.Log($"Class changed to: {newClass}");
    }

    private CharacterClassData GetClassData(CharacterClass targetClass)
    {
        foreach (CharacterClassData data in classDatas)
        {
            if (data.characterClass == targetClass)
            {
                return data;
            }
        }
        return null;
    }
}