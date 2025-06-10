using System.Collections;
using UnityEngine;

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
    public GameObject spell;
    public float attackDmg;

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

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _currentSpeed = walkSpeed;
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
            StartCoroutine(PerformAttack());
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            RegenerateMana();
        }

        // Update animator
        _animator.SetFloat("Speed", direction.magnitude * _currentSpeed);
        _animator.SetBool("IsGrounded", _isGrounded);
    }

    IEnumerator PerformAttack()
    {
        if (_currentMana >= 10)
        {
            _animator.SetTrigger("Attack");
            CastingSkill(10); // Cast skill and reduce mana
        }
        else
        {
            Debug.Log("Not enough mana to perform the attack.");
            yield break; // Exit if not enough mana
        }
        
        yield return new WaitForSeconds(2.05f); // Wait for the attack animation to play
        spell.SetActive(true); // Activate the spell object
        yield return new WaitForSeconds(1.5f);
        spell.SetActive(false); // Deactivate the spell object after the attack animation

        // Detect enemies in front
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange / 2, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Monster monster = enemy.GetComponent<Monster>();
            monster.TakeDamage(attackDmg);
            Debug.Log("Hit: " + enemy.name + " for " + attackDmg);
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
}