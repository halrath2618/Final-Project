using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStatsManager playerStatsManager; // Reference to the PlayerStatsManager script
    private bool _isAttacking = false; // Flag to check if the player is currently attacking
    public bool CanMove => !_isAttacking; // Property to check if the player can move

    private CharacterController _controller;
    private Animator _animator;


    [Header("Equipment")]
    public GameObject[] weapon; // Reference to the player's weapon GameObject

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 15f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Dodge Settings")]
    public float dodgeDistance = 5f;
    public float dodgeDuration = 0.4f;
    public float dodgeCooldown = 2f;
    public float dodgeInvulnerabilityDuration = 0.5f;
    public bool _isDodging = false;
    private bool _canDodge = true;
    private Coroutine _currentDodgeCoroutine;
    private bool _isInvulnerable = false;

    [Header("Combat Stats")]
    public float attackDmg;
    public float attackRange = 3f;
    public float attackRadius = 1.5f;
    public bool isAttacking = false;
    public LayerMask enemyLayer;
    [Header("After Meet Halrath: Skills and Spells")]
    public GameObject[] afterMeetHalrathSkillsIcon; // Reference to the After Meet Halrath's skills icons
    public GameObject afterMeetHalrathSkill1; // Reference to the After Meet Halrath's first skill GameObject
    public GameObject afterMeetHalrathSkill2; // Reference to the After Meet Halrath's second skill GameObject
    public GameObject afterMeetHalrathSkill3; // Reference to the After Meet Halrath's third skill GameObject
    public float attackDmg_AfterMeetHalrathSkill1 = 10f; // Damage for the first skill
    public float attackDmg_AfterMeetHalrathSkill2 = 15f; // Damage for the second skill

    [Header("MAge: Skills and Spells")]
    public GameObject[] mageSpellsIcon;
    public GameObject earthSpell;
    public GameObject fireSpell;
    public float attackDmg_Earth = 20f;
    public float attackDmg_Fire = 10f;
    public GameObject auraSpell;
    public GameObject healAura;
    public GameObject manaAura;

    [Header("Brawler: Skills and Spells")]
    public GameObject[] brawlerSkillsIcon; // Reference to the Brawler's skills icons
    public GameObject brawlerLeftHand; // Reference to the Brawler's first skill GameObject
    public GameObject brawlerRightHand; // Reference to the Brawler's second skill GameObject
    public GameObject brawlerLeftHand2; // Reference to the Brawler's second skill GameObject
    public GameObject brawlerSlash; // Reference to the Brawler's third skill GameObject
    public GameObject brawlerUltimate; // Reference to the Brawler's third skill GameObject
    public GameObject skill1HitArea; // Reference to the Brawler's first skill hit area GameObject
    public float brawlerSkill1Damage = 5f; // Damage for the first skill
    public float brawlerSkill2Damage = 25f; // Damage for the second skill
    public float brawlerUltimateDamage = 5f; // Damage for the ultimate skill

    [Header("SwordMaster: Skills and Spells")]
    public GameObject[] swordMasterSkillsIcon; // Reference to the SwordMaster's skills icons
    public GameObject swordMasterSkill1; // Reference to the SwordMaster's first skill GameObject
    public GameObject swordMasterSkill2; // Reference to the SwordMaster's second skill GameObject
    public GameObject swordMasterSkill3; // Reference to the SwordMaster's third skill GameObject
    public GameObject swordMasterSkill4; // Reference to the SwordMaster's fourth skill GameObject
    public GameObject swordMasterUltimate; // Reference to the SwordMaster's ultimate skill GameObject
    public float swordMasterSkill1Damage = 5f; // Damage for the first skill
    public float swordMasterSkill2Damage = 25f; // Damage for the second skill
    public float swordMasterSkill3Damage = 75f; // Damage for the third skill
    [Header("Audio")]
    public AudioSFXManager sfx; // Reference to the AudioSFXManager script
    
    
    public SphereCollider _attackCollider; // Collider for attack detection
    private Vector3 _velocity;
    private float _currentSpeed;
    private bool _isGrounded;
    private Transform _cameraTransform;

    [Header("HP, Mana and Stamina Control")]
    public float maxHP = 100;
    private HPBar hp;
    public float maxMana = 100;
    public float maxStamina = 100;
    public bool _isDrainingMana = false; // Flag to check if mana is being drained

    [Header("CoolDown Skill")]
    public SkillCoolDownManager skillCoolDownManager;
    //[Header("Cooldown Settings")]
    //public float skill1MaxCD; // Cooldown time for skills in seconds
    //public float skill1CDTime;
    //public bool skill1_isReady; // Flag to check if the skill is ready to be used
    private SkillCooldown skillCooldown;
    //public GameObject skill1_cdSlider;
    //public GameObject skill2_cdSlider;
    //public float skill2MaxCD; // Cooldown time for skills in seconds
    //public float skill2CDTime;
    //public bool skill2_isReady; // Flag to check if the skill is ready to be used
    //private Warning_Skill warningSkill;
    //private SkillConstantlyActive skillConstantlyActive;
    //public bool auraReady; // Flag to check if the aura skill is ready to be used
    //public GameObject HP_Potion;
    //public GameObject Mana_Potion;
    //public float hpMaxCD = 10f;
    //public float hpDuration = 10f;
    //public float manaDuration = 10f;
    //public float manaMaxCD = 10f;
    //public float hpcdTime = 10f;
    //public float manacdTime = 10f;
    //public bool hpcdReady = true; // Flag to check if the HP potion cooldown is ready
    //public bool manacdReady = true; // Flag to check if the Mana potion cooldown is ready

    [Header("Refs")]
    public Monster monster; // Reference to the Monster script for taking damage

    [Header("Notices")]
    public CanvasGroup noticeCanvasGroup; // Reference to the CanvasGroup for notices
    public GameObject noticePanel; // Reference to the notice panel GameObject

    [Header("Effects")]
    public GameObject fireHand; // Reference to the fire hand effect GameObject
    public GameObject fireEffect; // Reference to the aura effect GameObject

    [Header("Testing Switch Class")] //Testing swtiching class
    private CharacterClassManager characterClassManager; // Reference to the CharacterClassManager script
    public GameObject switchClassUI; // Reference to the UI GameObject for switching classes
    public void SwitchToBrawler()
    {
        brawlerSkillsIcon[0].SetActive(true);
        brawlerSkillsIcon[1].SetActive(true);
        brawlerSkillsIcon[2].SetActive(true);
        characterClassManager.SwitchClass(CharacterClass.Brawler); // Switch to Brawler class
        _controller.GetComponent<CharacterController>().enabled = true;
    }
    public void SwitchToMage()
    {
        mageSpellsIcon[0].SetActive(true); // Activate the Mage spell icon
        mageSpellsIcon[1].SetActive(true); // Activate the Mage spell icon
        mageSpellsIcon[2].SetActive(true); // Activate the Mage fire spell
        weapon[1].SetActive(true); // Activate the Mage weapon
        characterClassManager.SwitchClass(CharacterClass.Mage); // Switch to Mage class
        _controller.GetComponent<CharacterController>().enabled = true;
    }
    public void SwitchToSwordMaster()
    {
        swordMasterSkillsIcon[0].SetActive(true); // Activate the SwordMaster skill icon
        swordMasterSkillsIcon[1].SetActive(true); // Activate the SwordMaster skill icon
        swordMasterSkillsIcon[2].SetActive(true); // Activate the SwordMaster skill icon
        weapon[2].SetActive(true); // Activate the SwordMaster weapon
        characterClassManager.SwitchClass(CharacterClass.SwordMaster); // Switch to Rogue class
        _controller.GetComponent<CharacterController>().enabled = true;
    }
    public void SwitchToMeetHalrath()
    {
        afterMeetHalrathSkillsIcon[0].SetActive(true); // Activate the After Meet Halrath's first skill icon
        afterMeetHalrathSkillsIcon[1].SetActive(true); // Activate the After Meet Halrath's second skill icon
        weapon[0].SetActive(true);
        characterClassManager.SwitchClass(CharacterClass.After_Meet_Halrath); // Switch to Halrath class
        _controller.GetComponent<CharacterController>().enabled = true; // Enable character controller
    }
    void Start()
    {
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>(); // Find the PlayerStatsManager in the scene
        monster = FindAnyObjectByType<Monster>();
        skillCooldown = GetComponent<SkillCooldown>();
        skillCoolDownManager = GetComponent<SkillCoolDownManager>();
        characterClassManager = GetComponent<CharacterClassManager>();
        //warningSkill = GetComponent<Warning_Skill>();
        //skillConstantlyActive = GetComponent<SkillConstantlyActive>();
        hp = GetComponent<HPBar>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        //skillCooldown = GetComponent<SkillCooldown>();
        //switchClassUI.SetActive(true); // Show the switch class UI at the start
        //gameObject.GetComponent<CharacterController>().enabled = false; // Disable character controller until a class is selected
        _cameraTransform = Camera.main.transform;
        _currentSpeed = walkSpeed;
        //skill1_cdSlider.SetActive(false);
        auraSpell.SetActive(false);
        //auraReady = true;
    }

    void Update()
    {
        skillCoolDownManager.enabled = false;
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    playerStatsManager.characterClassNum = 1;
        //    switchClassUI.SetActive(false); // Hide the switch class UI after selection
        //}
        //else if (Input.GetKeyDown(KeyCode.N))
        //{
        //    playerStatsManager.characterClassNum = 2;
        //    switchClassUI.SetActive(false); // Hide the switch class UI after selection
        //}
        //else if (Input.GetKeyDown(KeyCode.M))
        //{
        //    playerStatsManager.characterClassNum = 3;
        //    switchClassUI.SetActive(false); // Hide the switch class UI after selection
        //}
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    playerStatsManager.characterClassNum = 4;
        //    switchClassUI.SetActive(false); // Hide the switch class UI after selection
        //}
        //if (playerStatsManager.characterClassNum == 1)
        //{
        //    skillCoolDownManager.skill1MaxCD = 1f; // Set cooldown time for Brawler's first skill
        //    skillCoolDownManager.skill1CDTime = skillCoolDownManager.skill1MaxCD; // Initialize cooldown time for Brawler's first skill
        //    skillCoolDownManager.skill2MaxCD = 4f; // Set cooldown time for Brawler's second skill
        //    skillCoolDownManager.skill2CDTime = skillCoolDownManager.skill2MaxCD; // Initialize cooldown time for Brawler's second skill
        //    SwitchToBrawler(); // Switch to Brawler class when 1 is pressed
        //}
        //else if (playerStatsManager.characterClassNum == 2)
        //{
        //    skillCoolDownManager.skill1MaxCD = 1f; // Set cooldown time for Mage's first skill
        //    skillCoolDownManager.skill1CDTime = skillCoolDownManager.skill1MaxCD; // Initialize cooldown time for Mage's first skill
        //    skillCoolDownManager.skill2MaxCD = 4f; // Set cooldown time for Mage's second skill
        //    skillCoolDownManager.skill2CDTime = skillCoolDownManager.skill2MaxCD; // Initialize cooldown time for Mage's second skill
        //    SwitchToMage(); // Switch to Mage class when 2 is pressed
        //}
        //else if (playerStatsManager.characterClassNum == 3)
        //{
        //    skillCoolDownManager.skill1MaxCD = 1f; // Set cooldown time for SwordMaster's first skill
        //    skillCoolDownManager.skill1CDTime = skillCoolDownManager.skill1MaxCD; // Initialize cooldown time for SwordMaster's first skill
        //    skillCoolDownManager.skill2MaxCD = 4f; // Set cooldown time for SwordMaster's second skill
        //    skillCoolDownManager.skill2CDTime = skillCoolDownManager.skill2MaxCD; // Initialize cooldown time for SwordMaster's second skill
        //    SwitchToSwordMaster(); // Switch to SwordMaster class when 3 is pressed
        //}
        //else if (playerStatsManager.characterClassNum == 4)
        //{
        //    skillCoolDownManager.skill1MaxCD = 1f; // Set cooldown time for After Meet Halrath's first skill
        //    skillCoolDownManager.skill1CDTime = skillCoolDownManager.skill1MaxCD; // Initialize cooldown time for After Meet Halrath's first skill
        //    skillCoolDownManager.skill2MaxCD = 2f; // Set cooldown time for After Meet Halrath's second skill
        //    skillCoolDownManager.skill2CDTime = skillCoolDownManager.skill2MaxCD; // Initialize cooldown time for After Meet Halrath's second skill
        //    SwitchToMeetHalrath(); // Switch to After Meet Halrath class when 4 is pressed
        //}
        skillCooldown.E_UpdateSkillCooldown();
        skillCooldown.F_UpdateSkillCooldown();
        hp.UpdateHP();
        hp.UpdateMana();
        hp.UpdateStamina();
        // Ground check
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;

        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (CanMove)
        {
            // Sprint
            if (Input.GetKey(KeyCode.LeftShift) && playerStatsManager.stamina > 0)
            {
                if (_currentSpeed <= 0)
                {
                    return;
                }
                else
                {
                    _currentSpeed = sprintSpeed;
                    StaminaSprinting();
                }
            }
            else
            {
                _currentSpeed = walkSpeed;
                StartCoroutine(RegenerateStamina());
            }

            // Camera-relative movement
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
                Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                _controller.Move(moveDir.normalized * _currentSpeed * Time.deltaTime);

                // Smooth rotation
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Jump
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                _animator.SetTrigger("Jump");
            }
        }

        // Dodge input
        if (Input.GetKeyDown(KeyCode.V) && _canDodge && !_isDodging && _isGrounded)
        {
            if (playerStatsManager.stamina < 10)
            {
                Debug.Log("Not enough stamina to dodge.");
                return; // Exit if not enough stamina
            }
            else
            {
                playerStatsManager.stamina -= 25; // Deduct stamina for dodging
                hp.UpdateStamina(); // Update stamina UI
                StartDodge();
            }

        }

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // Basic Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (skillCoolDownManager.skill1_isReady)
            {
                StartCoroutine(PerformAttack_1()); // Perform attack
                if (playerStatsManager.mana >= 10)
                {
                    skillCoolDownManager.skill1_isReady = false; // Set skill as not ready
                    StartCoroutine(skillCoolDownManager.ApplySkillCooldown1()); // Start cooldown
                }
                else
                {
                    skillCoolDownManager.skill1_isReady = true; // Reset skill readiness if not enough mana
                    return; // Exit if not enough mana
                }
            }
            else
            {
                StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                Debug.Log("Skill is not ready.");
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (skillCoolDownManager.skill2_isReady)
            {
                StartCoroutine(PerformAttack_2()); // Perform attack
                if (playerStatsManager.mana >= 30)
                {
                    skillCoolDownManager.skill2_isReady = false; // Set skill as not ready
                    StartCoroutine(skillCoolDownManager.ApplySkillCooldown2()); // Start cooldown
                }
                else
                {
                    skillCoolDownManager.skill2_isReady = true; // Reset skill readiness
                    return; // Exit if not enough mana
                }
            }
            else
            {
                
                StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                Debug.Log("Skill is not ready.");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerStatsManager.characterClassNum == 1)
            {
                if (skillCoolDownManager.auraReady)
                {
                    skillCoolDownManager.auraReady = false; // Set skill as not ready
                    StartCoroutine(PerformAttack_3()); // Perform attack
                    if (playerStatsManager.mana >= 80)
                    {
                        skillCoolDownManager.auraReady = true; // Reset skill readiness
                    }
                    else
                    {
                        skillCoolDownManager.auraReady = true; // Reset skill readiness
                        return; // Exit if not enough mana
                    }
                }
                else
                {
                    
                    StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                    Debug.Log("Skill is not ready.");
                }
            }
            else if (playerStatsManager.characterClassNum == 2)
            {
                if (skillCoolDownManager.auraReady)
                {
                    skillCoolDownManager.auraReady = false; // Set aura skill as not ready
                    _isDrainingMana = true; // Start draining mana
                    StartCoroutine(AuraManaDrainPerSecond()); // Start mana drain coroutine
                    auraSpell.SetActive(true); // Activate the aura spell
                    skillCoolDownManager.skillConstantlyActive.skillEffect.SetActive(true); // Activate the skill effect
                }
                else
                {
                    skillCoolDownManager.auraReady = true; // Reset skill readiness
                    _isDrainingMana = false; // Stop draining mana
                    auraSpell.SetActive(false); // Deactivate the aura spell
                    skillCoolDownManager.skillConstantlyActive.skillEffect.SetActive(false); // Deactivate the skill effect
                    StopDrainingMana(); // Stop draining mana if already active
                }
            }
            else if (playerStatsManager.characterClassNum == 3)
            {
                if (skillCoolDownManager.auraReady)
                {
                    skillCoolDownManager.auraReady = false; // Set skill as not ready
                    StartCoroutine(PerformAttack_3()); // Perform attack
                    if (playerStatsManager.mana >= 80)
                    {
                        skillCoolDownManager.auraReady = true; // Reset skill readiness
                    }
                    else
                    {
                        skillCoolDownManager.auraReady = true; // Reset skill readiness
                        return; // Exit if not enough mana
                    }
                }
                else
                {
                    
                    StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                    Debug.Log("Skill is not ready.");
                }
            }
        }
        //Regenerate Mana
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (skillCoolDownManager.manacdReady && playerStatsManager.MPPotionCount > 0)
            {
                playerStatsManager.MPPotionCount--; // Decrease Mana potion count
                skillCoolDownManager.manacdReady = false; // Set Mana potion as not ready
                StartCoroutine(skillCoolDownManager.ApplySkillCooldownManaPotion()); // Start cooldown for Mana potion
                StartCoroutine(RegenerateMana()); // Regenerate mana
            }
            else if (!skillCoolDownManager.manacdReady)
            {
                Debug.Log("Mana potion is on cooldown.");
            }
            else if (playerStatsManager.MPPotionCount <= 0)
            {
                playerStatsManager.MPPotionCount = 0; // Ensure Mana potion count does not go below zero
                Debug.Log("No Mana potions left.");
            }
        }
        // Regenerate HP
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Check if E key is pressed and HP potion is ready
        {
            if (skillCoolDownManager.hpcdReady && playerStatsManager.HPPotionCount > 0)
            {
                playerStatsManager.HPPotionCount--; // Decrease HP potion count
                skillCoolDownManager.hpcdReady = false; // Set HP potion as not ready
                StartCoroutine(skillCoolDownManager.ApplySkillCooldownHPPotion()); // Start cooldown for HP potion
                StartCoroutine(DrinkHPPotion()); // Drink HP potion
            }
            else if (!skillCoolDownManager.hpcdReady)
            {
                Debug.Log("HP potion is on cooldown.");
            }
            else if (playerStatsManager.HPPotionCount <= 0)
            {
                playerStatsManager.HPPotionCount = 0; // Ensure HP potion count does not go below zero
                Debug.Log("No HP potions left.");
            }

        }
        // Update animator
        _animator.SetFloat("Speed", direction.magnitude * _currentSpeed);
        _animator.SetBool("IsGrounded", _isGrounded);
    }

    void StartDodge()
    {
        if (playerStatsManager.stamina > 10)
        {
            // Cancel any ongoing attacks
            CancelAttacks();
            // Start dodge coroutine
            if (_currentDodgeCoroutine != null)
            {
                StopCoroutine(_currentDodgeCoroutine);
            }
            _currentDodgeCoroutine = StartCoroutine(PerformDodge());
        }
        else
        {
            Debug.Log("Not enough stamina to dodge.");
            return; // Exit if not enough stamina
        }
    }

    void CancelAttacks()
    {
        // Cancel attack animations and effects
        _animator.ResetTrigger("Attack1");
        _animator.ResetTrigger("Attack2");

        // Reset attack states
        isAttacking = false;
        _isAttacking = false;
    }

    IEnumerator PerformDodge()
    {
        _isDodging = true;
        _canDodge = false;
        _isInvulnerable = true;

        // Trigger dodge animation
        _animator.SetTrigger("Dodge");

        // Calculate dodge direction
        Vector3 dodgeDirection = transform.forward;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            // Dodge in movement direction
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            dodgeDirection = new Vector3(horizontal, 0, vertical).normalized;

            // Convert to camera-relative direction
            float targetAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            dodgeDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        }

        // Dodge movement
        float elapsed = 0f;
        while (elapsed < dodgeDuration)
        {
            float progress = elapsed / dodgeDuration;
            float currentDistance = Mathf.Lerp(dodgeDistance, 0, progress);

            _controller.Move(dodgeDirection * currentDistance * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // End invulnerability after dodge
        yield return new WaitForSeconds(dodgeInvulnerabilityDuration);
        _isInvulnerable = false;

        _isDodging = false;

        // Start cooldown
        yield return new WaitForSeconds(dodgeCooldown);
        _canDodge = true;
    }

    IEnumerator DrinkHPPotion()
    {
        while (skillCoolDownManager.hpDuration > 0)
        {
            yield return playerStatsManager.health += 5 * Time.deltaTime;
            skillCoolDownManager.hpDuration -= Time.deltaTime; // Decrease duration of HP potion effect
            healAura.SetActive(true); // Activate the heal aura effect

        }
        healAura.SetActive(false); // Deactivate the heal aura effect
        if (playerStatsManager.health > maxHP)
        {
            playerStatsManager.health = maxHP; // Ensure health does not exceed max HP
            Debug.Log("Health is already full.");
        }
        hp.UpdateHP();
        skillCoolDownManager.hpDuration = 10f; // Reset duration for the next use
    }
    public void StopDrainingMana()
    {
        if (!_isDrainingMana)
        {
            StopCoroutine(AuraManaDrainPerSecond()); // Stop the mana drain coroutine
        }
    }
    IEnumerator AuraManaDrainPerSecond()
    {
        while (playerStatsManager.mana > 0 && _isDrainingMana)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            playerStatsManager.mana -= 1; // Drain 1 mana per second
            hp.UpdateMana(); // Update mana UI
            if (playerStatsManager.mana <= 0)
            {
                playerStatsManager.mana = 0; // Ensure mana does not go below zero
                skillCoolDownManager.auraReady = true; // Reset skill readiness
                auraSpell.SetActive(false); // Deactivate the aura spell
                skillCoolDownManager.skillConstantlyActive.skillEffect.SetActive(false); // Deactivate the skill effect
                Debug.Log("Aura skill deactivated due to no mana.");
                break; // Exit the coroutine if no mana left
            }
        }
    }
    IEnumerator PerformAttack_3()
    {
        if (playerStatsManager.characterClassNum == 2)
        {
            _isInvulnerable = true; // Set invulnerability during the attack
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 80)
                    {
                        _animator.SetTrigger("Attack3");
                        CastingSkill(80); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    brawlerUltimate.SetActive(true); // Activate the ultimate effect
                    _attackCollider.enabled = true; // Enable the attack collider for hit detection
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.01f); // Wait for the attack animation to play
                    _attackCollider.enabled = false; // Disable the attack collider after the attack animation
                    yield return new WaitForSeconds(0.83f); // Wait for the attack animation to play
                    _attackCollider.enabled = true;
                    brawlerUltimate.SetActive(false); // Deactivate the ultimate effect after the attack animation
                    _isInvulnerable = false; // Reset invulnerability after the attack
                }
                else
                {
                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
        else if (playerStatsManager.characterClassNum == 4)
        {
            _isInvulnerable = true; // Set invulnerability during the attack
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 80)
                    {
                        _animator.SetTrigger("Attack3");
                        CastingSkill(80); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    yield return new WaitForSeconds(1.2f); // Wait for the attack animation to play
                    swordMasterUltimate.SetActive(true); // Activate the ultimate effect
                    yield return new WaitForSeconds(1.1f); // Wait for the attack animation to play
                    swordMasterUltimate.SetActive(false); // Deactivate the ultimate effect after the attack animation
                }
                else
                {
                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
    }
    IEnumerator PerformAttack_1()
    {
        if (playerStatsManager.characterClassNum == 2)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 3)
                    {
                        _animator.SetTrigger("Attack1");
                        CastingSkill(3); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    brawlerLeftHand.SetActive(true); // Activate the left hand effect
                    skill1HitArea.SetActive(true); // Activate the hit area for the first skill
                    yield return new WaitForSeconds(0.25f); // Wait for the attack animation to play
                    skill1HitArea.SetActive(false); // Deactivate the hit area after the attack animation
                    brawlerLeftHand.SetActive(false); // Deactivate the left hand effect after the attack animation
                    brawlerRightHand.SetActive(true); // Activate the right hand effect
                    skill1HitArea.SetActive(true); // Reactivate the hit area for the second skill
                    yield return new WaitForSeconds(0.25f); // Wait for the attack animation to play
                    skill1HitArea.SetActive(false); // Deactivate the hit area after the attack animation
                    brawlerRightHand.SetActive(false);
                    brawlerLeftHand2.SetActive(true); // Reactivate the left hand effect
                    skill1HitArea.SetActive(true); // Reactivate the hit area for the third skill
                    yield return new WaitForSeconds(0.25f); // Wait for the attack animation to play
                    skill1HitArea.SetActive(false); // Deactivate the hit area after the attack animation
                    brawlerLeftHand2.SetActive(false); // Deactivate the left hand effect after the attack animation
                }
                else
                {
                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
        else if (playerStatsManager.characterClassNum == 3)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 10)
                    {
                        _animator.SetTrigger("Attack1");
                        CastingSkill(10); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }

                    yield return new WaitForSeconds(1.025f); // Wait for the attack animation to play
                    earthSpell.SetActive(true); // Activate the spell object
                    yield return new WaitForSeconds(1.5f);
                    earthSpell.SetActive(false); // Deactivate the spell object after the attack animation
                }
                else
                {
                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
        else if (playerStatsManager.characterClassNum == 4)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 3)
                    {
                        _animator.SetTrigger("Attack1");
                        CastingSkill(3); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    swordMasterSkill1.SetActive(true); // Activate the skill effect
                    yield return new WaitForSeconds(0.26f); // Wait for the attack animation to play
                    swordMasterSkill2.SetActive(true); // Activate the second skill effect
                    yield return new WaitForSeconds(0.26f); // Wait for the attack animation to play
                    swordMasterSkill3.SetActive(true); // Activate the third skill effect
                    yield return new WaitForSeconds(0.26f); // Wait for the attack animation to play
                    swordMasterSkill1.SetActive(false); // Deactivate the skill effect after the attack animation
                    swordMasterSkill2.SetActive(false); // Deactivate the second skill effect after the attack animation
                    swordMasterSkill3.SetActive(false); // Deactivate the third skill effect after the attack animation
                }
                else
                {
                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
        else if (playerStatsManager.characterClassNum == 1)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 3)
                    {
                        _animator.SetTrigger("Attack1");
                        CastingSkill(3); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    yield return new WaitForSeconds(0.5f); // Wait for the attack animation to play
                    afterMeetHalrathSkill1.SetActive(true); // Activate the skill effect
                    yield return new WaitForSeconds(1.1f); // Wait for the attack animation to play
                    afterMeetHalrathSkill1.SetActive(false); // Deactivate the skill effect after the attack animation
                }
                else
                {

                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
    }
    IEnumerator PerformAttack_2()
    {
        if (playerStatsManager.characterClassNum == 2)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 8)
                    {
                        _animator.SetTrigger("Attack2");
                        CastingSkill(8); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    yield return new WaitForSeconds(0.35f); // Wait for the attack animation to play
                    brawlerSlash.SetActive(true); // Activate the slash effect
                    yield return new WaitForSeconds(0.5f); // Wait for the attack animation to play
                    brawlerSlash.SetActive(false); // Deactivate the slash effect after the attack animation
                }
                else
                {

                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
        else if (playerStatsManager.characterClassNum == 3)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 30)
                    {
                        _animator.SetTrigger("Attack2");
                        CastingSkill(30); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    fireHand.SetActive(true); // Activate the fire hand effect
                    fireEffect.SetActive(true); // Activate the fire effect
                    yield return new WaitForSeconds(1.025f); // Wait for the attack animation to play
                    fireSpell.SetActive(true); // Activate the spell object
                    yield return new WaitForSeconds(2.75f);
                    fireSpell.SetActive(false); // Deactivate the spell object after the attack animation
                    fireHand.SetActive(false); // Deactivate the fire hand effect
                    fireEffect.SetActive(false); // Deactivate the fire effect
                }
                else
                {

                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
        else if (playerStatsManager.characterClassNum == 4)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 10)
                    {
                        _animator.SetTrigger("Attack2");
                        CastingSkill(30); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    swordMasterSkill4.SetActive(true); // Activate the skill effect
                    yield return new WaitForSeconds(1.1f); // Wait for the attack animation to play
                    swordMasterSkill4.SetActive(false); // Deactivate the skill effect after the attack animation
                }
                else
                {

                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
        else if (playerStatsManager.characterClassNum == 1)
        {
            _isAttacking = true; // Set attacking flag to true
            try
            {
                if (!isAttacking)
                {

                    if (playerStatsManager.mana >= 3)
                    {
                        _animator.SetTrigger("Attack2");
                        CastingSkill(3); // Cast skill and reduce mana
                    }
                    else
                    {
                        
                        StartCoroutine(skillCoolDownManager.warningSkill.Flashing()); // Start flashing warning
                        Debug.Log("Not enough mana to perform the attack.");
                        yield break; // Exit if not enough mana
                    }
                    afterMeetHalrathSkill2.SetActive(true); // Activate the skill effect
                    yield return new WaitForSeconds(0.6f); // Wait for the attack animation to play
                    afterMeetHalrathSkill3.SetActive(true); // Deactivate the skill effect after the attack animation
                    yield return new WaitForSeconds(0.6f); // Wait for the attack animation to play
                    afterMeetHalrathSkill2.SetActive(false); // Deactivate the skill effect after the attack animation
                    afterMeetHalrathSkill3.SetActive(false); // Deactivate the skill effect after the attack animation

                }
                else
                {

                    Debug.Log("Already attacking, please wait.");
                    yield break; // Exit if already attacking
                }
            }
            finally
            {
                _isAttacking = false; // Reset attacking flag
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (_isInvulnerable) return; // Ignore damage during dodge

        playerStatsManager.health -= damage;
        hp.UpdateHP(); // Update health bar UI
        if (playerStatsManager.health <= 0)
        {
            playerStatsManager.health = 0; // Ensure health does not go below zero
            StartCoroutine(Die()); // Trigger game over sequence
        }
    }
    IEnumerator GameOver()
    {
        noticePanel.SetActive(true); // Show the notice panel
        noticeCanvasGroup.alpha += 0.5f * Time.deltaTime; // Set the alpha to fully visible
        yield return new WaitForSeconds(3f); // Wait for 2 seconds
    }
    IEnumerator Die()
    {
        StartCoroutine(GameOver()); // Start the game over sequence
        GetComponent<CharacterController>().enabled = false; // Disable character controller to stop movement
        _animator.SetTrigger("Die");
        Debug.Log("Player has died.");
        yield return new WaitForSeconds(5f); // Wait for death animation to finish
        SceneManager.LoadScene("LoadingScene"); // Load Game Over scene
    }
    public void CastingSkill(int manaCost)
    {
        if (playerStatsManager.mana >= manaCost)
        {
            playerStatsManager.mana -= manaCost;
            hp.UpdateMana();
            // Add skill casting logic here
        }
        else
        {
            Debug.Log("Not enough mana to cast the skill.");
        }
    }
    public void StaminaSprinting()
    {
        if (playerStatsManager.stamina > 0 && _currentSpeed >= sprintSpeed)
        {
            playerStatsManager.stamina -= 0.5f; // Decrease stamina for sprinting
            hp.UpdateStamina();
        }
        else
        {
            Debug.Log("Not enough stamina to sprint.");
            _currentSpeed = walkSpeed; // Reset speed if no stamina
        }
    }
    IEnumerator RegenerateStamina()
    {

        yield return new WaitForSeconds(5f * Time.deltaTime);
        yield return playerStatsManager.stamina += 1;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            yield return new WaitForSeconds(5f * Time.deltaTime); // Wait for the next frame to continue regenerating stamina
        }
        if (playerStatsManager.stamina > maxStamina)
            playerStatsManager.stamina = maxStamina;
        hp.UpdateStamina();
    }

    IEnumerator RegenerateMana()
    {
        if (playerStatsManager.mana < maxMana)
        {
            while (skillCoolDownManager.manaDuration > 0)
            {
                yield return playerStatsManager.mana += 10 * Time.deltaTime; // Regenerate mana
                skillCoolDownManager.manaDuration -= Time.deltaTime; // Decrease duration of mana regeneration effect
                manaAura.SetActive(true); // Activate the heal aura effect
            }
            manaAura.SetActive(false); // Deactivate the heal aura effect after regeneration
            if (playerStatsManager.mana > maxMana)
                playerStatsManager.mana = maxMana; // Ensure mana does not exceed max mana
            hp.UpdateMana();
            skillCoolDownManager.manaDuration = 10f;
        }
        else
        {
            Debug.Log("Mana is already full.");
        }
    }
    //IEnumerator ApplySkillCooldown1()
    //{
    //    while (skill1CDTime > 0)
    //    {
    //        skill1_cdSlider.SetActive(true);
    //        yield return skill1CDTime -= Time.deltaTime; // Decrease cooldown time
    //        if (skill1CDTime <= 0)
    //        {
    //            skill1CDTime = 0; // Ensure cooldown does not go below zero
    //            skill1_isReady = true; // Skill is ready again
    //            skill1_cdSlider.SetActive(false); // Hide cooldown slider
    //            break;
    //        }
    //    }
    //    skill1CDTime = skill1MaxCD; // Reset cooldown time
    //    skillCooldown.E_UpdateSkillCooldown(); // Update the cooldown slider UI
    //}
    //IEnumerator ApplySkillCooldown2()
    //{
    //    while (skill2CDTime > 0)
    //    {
    //        Debug.Log("Skill 2 cooldown: " + skill2CDTime);
    //        skill2_cdSlider.SetActive(true);
    //        yield return skill2CDTime -= Time.deltaTime; // Decrease cooldown time
    //        if (skill2CDTime <= 0)
    //        {
    //            skill2CDTime = 0; // Ensure cooldown does not go below zero
    //            skill2_isReady = true; // Skill is ready again
    //            skill2_cdSlider.SetActive(false); // Hide cooldown slider
    //            break;
    //        }
    //    }
    //    Debug.Log("Skill 2 cooldown finished.");
    //    skill2CDTime = skill2MaxCD; // Reset cooldown time
    //    skillCooldown.F_UpdateSkillCooldown(); // Update the cooldown slider UI
    //}
    //IEnumerator ApplySkillCooldownHPPotion()
    //{
    //    while (hpcdTime > 0)
    //    {
    //        HP_Potion.SetActive(true);
    //        yield return hpcdTime -= Time.deltaTime; // Decrease cooldown time
    //        if (hpcdTime <= 0)
    //        {
    //            hpcdTime = 0; // Ensure cooldown does not go below zero
    //            hpcdReady = true; // Skill is ready again
    //            HP_Potion.SetActive(false); // Hide cooldown slider
    //            break;
    //        }
    //    }
    //    hpcdTime = hpMaxCD; // Reset cooldown time
    //    skillCooldown.HPCooldownUpdate(); // Update the cooldown slider UI
    //}
    //IEnumerator ApplySkillCooldownManaPotion()
    //{
    //    while (manacdTime > 0)
    //    {
    //        Mana_Potion.SetActive(true);
    //        yield return manacdTime -= Time.deltaTime; // Decrease cooldown time
    //        if (manacdTime <= 0)
    //        {
    //            manacdTime = 0; // Ensure cooldown does not go below zero
    //            manacdReady = true; // Skill is ready again
    //            Mana_Potion.SetActive(false); // Hide cooldown slider
    //            break;
    //        }
    //    }
    //    manacdTime = manaMaxCD; // Reset cooldown time
    //    skillCooldown.MPCooldownUpdate(); // Update the cooldown slider UI
    //}
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Claw"))
        {
            Debug.Log("Player hit by monster claw");
            TakeDamage(monster.attackDamage); // Assuming Monster.Health has a static attackDmg variable
        }
    }
}