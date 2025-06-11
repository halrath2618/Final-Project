using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 15f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Combat")]
    public float attackRange = 3f;
    public float attackRadius = 1.5f;
    public LayerMask enemyLayer;
    public GameObject earthSpell;
    public GameObject fireSpell;
    public float attackDmg_Earth;
    public float attackDmg_Fire;
    public bool isAttacking = false;
    public GameObject auraSpell;

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator _animator;
    private Vector3 _velocity;
    private float _currentSpeed;
    private bool _isGrounded;
    private Transform _cameraTransform;

    [Header("HP, Mana and Stamina Control")]
    public int maxHP = 100;
    public int _currentHealth = 100;
    public HPBar hp;
    public int maxMana = 100;
    public int _currentMana = 100;
    public ManaStamina manaStamina;
    public int maxStamina = 100;
    public float _currentStamina = 100;

    [Header("Cooldown Settings")]
    public float e_maxSkillCooldown = 4f; // Cooldown time for skills in seconds
    public float e_cdTime = 4f;
    public bool e_isReady; // Flag to check if the skill is ready to be used
    public SkillCooldown skillCooldown;
    public GameObject E_cdSlider;
    public GameObject F_cdSlider;
    public float f_maxSkillCooldown = 6f; // Cooldown time for skills in seconds
    public float f_cdTime = 6f;
    public bool f_isReady; // Flag to check if the skill is ready to be used
    public Warning_Skill warningSkill;
    public SkillConstantlyActive skillConstantlyActive;
    public bool auraReady; // Flag to check if the aura skill is ready to be used
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _currentSpeed = walkSpeed;
        E_cdSlider.SetActive(false);
        auraSpell.SetActive(false);
        auraReady = true;
    }

    void Update()
    {
        
        hp.HP();
        manaStamina.UpdateMana();
        manaStamina.UpdateStamina();
        // Ground check
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;

        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Sprint
        if(Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0)
        {
            _currentSpeed = sprintSpeed;
            StaminaSprinting();
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

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // Basic Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (e_isReady)
            {
                e_isReady = false; // Set skill as not ready
                StartCoroutine(PerformAttack_1()); // Perform attack
                if (_currentMana >= 10)
                {
                    StartCoroutine(ApplySkillCooldownE()); // Start cooldown
                }
                else
                {
                    e_isReady = true; // Reset skill readiness if not enough mana
                    return; // Exit if not enough mana
                }
            }
            else
            {
                warningSkill.gameWarning.SetActive(true); // Show warning for skill cooldown
                StartCoroutine(warningSkill.Flashing()); // Start flashing warning
                Debug.Log("Skill is not ready.");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (f_isReady)
            {
                f_isReady = false; // Set skill as not ready
                StartCoroutine(PerformAttack_2()); // Perform attack
                if (_currentMana >= 30)
                {
                    StartCoroutine(ApplySkillCooldownF()); // Start cooldown
                }
                else
                {
                    f_isReady = true; // Reset skill readiness
                    return; // Exit if not enough mana
                }
            }
            else
            {
                warningSkill.gameWarning.SetActive(true); // Show warning for skill cooldown
                StartCoroutine(warningSkill.Flashing()); // Start flashing warning
                Debug.Log("Skill is not ready.");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(auraReady)
            {
                auraReady = false; // Set skill as not ready
                auraSpell.SetActive(true); // Activate the aura spell
                StartCoroutine(skillConstantlyActive.SkillActively()); // Start the skill effect coroutine
                skillConstantlyActive.skillEffect.SetActive(true); // Activate the skill effect
                Debug.Log("Aura skill activated.");
            }
            else
            {
                skillConstantlyActive.skillEffect.SetActive(false); // Deactivate the skill effect
                auraReady = true; // Reset skill readiness
                auraSpell.SetActive(false); // Deactivate the aura spell
                Debug.Log("Aura skill deactivated.");
            }
        }

        //Regenerate Mana
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RegenerateMana();
        }

        // Update animator
        _animator.SetFloat("Speed", direction.magnitude * _currentSpeed);
        _animator.SetBool("IsGrounded", _isGrounded);
    }

    IEnumerator PerformAttack_1()
    {
        if (!isAttacking)
        {
           
            if (_currentMana >= 10)
            {
                _animator.SetTrigger("Attack1");
                CastingSkill(10); // Cast skill and reduce mana
            }
            else
            {
                Debug.Log("Not enough mana to perform the attack.");
                yield break; // Exit if not enough mana
            }

            yield return new WaitForSeconds(2.05f); // Wait for the attack animation to play
            earthSpell.SetActive(true); // Activate the spell object
            yield return new WaitForSeconds(1.5f);
            earthSpell.SetActive(false); // Deactivate the spell object after the attack animation
        }
        else
        {
            Debug.Log("Already attacking, please wait.");
            yield break; // Exit if already attacking
        }

        // Detect enemies in front
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange / 2, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Monster monster = enemy.GetComponent<Monster>();
            monster.TakeDamage(attackDmg_Earth);
            Debug.Log("Hit: " + enemy.name + " for " + attackDmg_Earth);
        }
    }

    IEnumerator PerformAttack_2()
    {
        if (!isAttacking)
        {
           
            if (_currentMana >= 30)
            {
                _animator.SetTrigger("Attack2");
                CastingSkill(30); // Cast skill and reduce mana
            }
            else
            {
                Debug.Log("Not enough mana to perform the attack.");
                yield break; // Exit if not enough mana
            }

            yield return new WaitForSeconds(2.05f); // Wait for the attack animation to play
            fireSpell.SetActive(true); // Activate the spell object
            yield return new WaitForSeconds(2.75f);
            fireSpell.SetActive(false); // Deactivate the spell object after the attack animation
        }
        else
        {
            
            Debug.Log("Already attacking, please wait.");
            yield break; // Exit if already attacking
        }

            // Detect enemies in front
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange / 2, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Monster monster = enemy.GetComponent<Monster>();
            monster.TakeDamage(attackDmg_Fire);
            Debug.Log("Hit: " + enemy.name + " for " + attackDmg_Fire);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + transform.forward * attackRange / 2,
            attackRadius
        );
    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        _animator.SetTrigger("Die");
        Debug.Log("Player has died.");
        _currentHealth = maxHP; // Reset health for respawn
    }
    public void CastingSkill(int manaCost)
    {
        if (_currentMana >= manaCost)
        {
            _currentMana -= manaCost;
            manaStamina.UpdateMana();
            // Add skill casting logic here
        }
        else
        {
            Debug.Log("Not enough mana to cast the skill.");
        }
    }
    public void StaminaSprinting()
    {
        if (_currentStamina > 0 && _currentSpeed >= sprintSpeed)
        {
            _currentStamina -= 0.5f; // Decrease stamina for sprinting
            manaStamina.UpdateStamina();
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
        yield return _currentStamina += 1;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            yield return new WaitForSeconds(5f * Time.deltaTime); // Wait for the next frame to continue regenerating stamina
        }
        if (_currentStamina > maxStamina) 
            _currentStamina = maxStamina;
        manaStamina.UpdateStamina();
    }

    public void RegenerateMana()
    {
        if (_currentMana < maxMana)
        {
            _currentMana += 10; // Regenerate mana
            manaStamina.UpdateMana();
        }
        else
        {
            Debug.Log("Mana is already full.");
        }
    }
    IEnumerator ApplySkillCooldownE()
    {
        while (e_cdTime > 0)
        {
            E_cdSlider.SetActive(true);
            yield return e_cdTime -= Time.deltaTime; // Decrease cooldown time
            if (e_cdTime <= 0)
            {
                e_cdTime = 0; // Ensure cooldown does not go below zero
                e_isReady = true; // Skill is ready again
                E_cdSlider.SetActive(false); // Hide cooldown slider
                break;
            }
        }
        e_cdTime = e_maxSkillCooldown; // Reset cooldown time
        skillCooldown.E_UpdateSkillCooldown(); // Update the cooldown slider UI
    }
    IEnumerator ApplySkillCooldownF()
    {
        while (f_cdTime > 0)
        {
            F_cdSlider.SetActive(true);
            yield return f_cdTime -= Time.deltaTime; // Decrease cooldown time
            if (f_cdTime <= 0)
            {
                f_cdTime = 0; // Ensure cooldown does not go below zero
                f_isReady = true; // Skill is ready again
                F_cdSlider.SetActive(false); // Hide cooldown slider

                break;
            }
        }
        f_cdTime = f_maxSkillCooldown; // Reset cooldown time
        skillCooldown.F_UpdateSkillCooldown(); // Update the cooldown slider UI
    }
}