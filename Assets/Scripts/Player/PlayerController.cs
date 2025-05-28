using UnityEngine;
using UnityEngine.AI;

public class PlayerAutoAttack : MonoBehaviour
{
    [Header("Auto–Attack Settings")]
    [Tooltip("The detection radius used when searching for monsters.")]
    public float detectionRadius = 50f;
    [Tooltip("The range at which the player starts to attack the monster.")]
    public float attackRange = 2f;
    [Tooltip("Damage dealt per attack.")]
    public int attackDamage = 15;
    [Tooltip("Time cooldown (in seconds) between attacks.")]
    public float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

    [Header("Health Settings")]
    [Tooltip("Player’s maximum HP.")]
    public int maxHP = 100;
    [Tooltip("Player’s current HP.")]
    public int currentHP;

    [Header("References")]
    [Tooltip("Reference to the NavMeshAgent used for navigation.")]
    public NavMeshAgent agent;
    [Tooltip("Optional: An Attack Point that serves as the origin of the attack.")]
    public Transform attackPoint;

    // The current target monster (cached as a Transform).
    private Transform targetMonster;

    [SerializeField] private Animator anim;  // Animator for player animations

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (attackPoint == null)
            attackPoint = transform;  // fallback to the player's transform

        currentHP = maxHP;  // initialize player health
    }

    void Update()
    {
        // Find a target monster if there is none or if the previous target is no longer active.
        if (targetMonster == null || !targetMonster.gameObject.activeInHierarchy)
        {
            FindTarget();
        }

        if (targetMonster != null)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude); // Update animation speed based on agent velocity
            // Update the NavMeshAgent's path to the target.
            agent.SetDestination(targetMonster.position);

            // Smoothly rotate the player to face the target.
            Vector3 direction = (targetMonster.position - transform.position).normalized;
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Check the distance and perform an attack if within range.
            float distanceToTarget = Vector3.Distance(transform.position, targetMonster.position);
            if (distanceToTarget <= attackRange)
            {
                // Stop moving when target is close enough
                agent.ResetPath();

                // Attack if attack cooldown has elapsed.
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
    }


    /// Searches for the closest monster within the detection radius.

    void FindTarget()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude); // Update animation speed based on agent velocity
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        float closestDistance = Mathf.Infinity;
        Transform closestMonster = null;

        foreach (GameObject m in monsters)
        {
            float distance = Vector3.Distance(transform.position, m.transform.position);
            if (distance < closestDistance && distance <= detectionRadius)
            {
                closestDistance = distance;
                closestMonster = m.transform;
            }
        }

        targetMonster = closestMonster;
    }


    /// Executes an attack on all colliders within the attack range.
    void Attack()
    {
        Debug.Log("Player auto-attacks!");
        anim.SetTrigger("Attack"); // Trigger attack animation
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider hit in hitColliders)
        {
            // Check if the hit object is a monster.
            if (hit.CompareTag("Monster"))
            {
                Monster enemy = hit.GetComponent<Monster>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    Debug.Log("Monster hit for " + attackDamage + " damage!");
                }
            }
        }
    }
    /// Applies damage to the player, reducing current HP. Calls Die() if HP reaches 0.
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Player takes " + damage + " damage, current HP: " + currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }


    /// Handles player death.

    void Die()
    {
        Debug.Log("Player has died!");
        anim.SetTrigger("Die"); // Trigger die animation
    }

    // For debugging: Visualize attack and detection ranges in the Scene view.
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}