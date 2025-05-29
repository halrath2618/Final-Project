using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float currentHP = 100f;

    [Header("Damage Settings")]
    public int normalAttackDamage = 10;
    public int strongAttackDamage = 20;

    [Header("Attack Settings")]
    public float attackRange = 2f;  // Radius for collider check on attack

    [Header("Movement Settings")]
    public float runSpeed;
    public Transform player;  // Assign your Player transform in the Inspector
    [SerializeField] private Animator anim;  //Animator for monster animations

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public NavMeshAgent agent;

    private BehaviorTree behaviorTree;

    void Start()
    {
        // Get the NavMeshAgent attached to this monster
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0 && agent != null)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        // ------- Build the Behavior Tree -------
        // Health-based attack sequences:
        var runAwaySequence = new SequenceNode(new List<BTNode>
        {
            new HealthConditionNode(this, 0.5f, true),   // currentHP <= 50%
            new RunAwayActionNode(this)
        });

        var strongAttackSequence = new SequenceNode(new List<BTNode>
        {
            new HealthConditionNode(this, 0.7f, true),    // currentHP < 70%
            new HealthConditionNode(this, 0.5f, false),     // currentHP > 50%
            new StrongAttackActionNode(this)
        });

        var normalAttackSequence = new SequenceNode(new List<BTNode>
        {
            new HealthConditionNode(this, 0.7f, false),     // currentHP > 70%
            new AttackActionNode(this)
        });

        // Selector to choose the attack based on health
        var attackSelector = new SelectorNode(new List<BTNode>
        {
            runAwaySequence,
            strongAttackSequence,
            normalAttackSequence
        });

        // Attack branch: only active if the player is within 20 units.
        var attackBranch = new SequenceNode(new List<BTNode>
        {
            new PlayerInRangeConditionNode(this, 20f),
            attackSelector
        });

        // The root: if the player isn’t near, the monster will patrol.
        behaviorTree = new BehaviorTree(new List<BTNode>
        {
            attackBranch,
            new PatrolActionNode(this)
        });
    }

    void Update()
    {
        // Evaluate behavior tree every frame
        behaviorTree.Evaluate();
    }

    // ---------- Action Methods ----------

    public void NormalAttack()
    {
        Debug.Log("Monster performs NORMAL attack!");
        anim.SetTrigger("Attack"); // Trigger attack animation
        // Use OverlapSphere for a simple attack collision check.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerAutoAttack playerScript = hit.GetComponent<PlayerAutoAttack>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(normalAttackDamage);
                    Debug.Log("Player hit for " + normalAttackDamage + " damage (normal attack).");
                }
            }
        }
    }

    public void StrongAttack()
    {
        Debug.Log("Monster performs STRONG attack!");
        anim.SetTrigger("Attack"); // Trigger strong attack animation
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerAutoAttack playerScript = hit.GetComponent<PlayerAutoAttack>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(strongAttackDamage);
                    Debug.Log("Player hit for " + strongAttackDamage + " damage (strong attack).");
                }
            }
        }
    }

    public void RunAway()
    {
        Debug.Log("Monster runs away from the player!");
        anim.SetFloat("Speed", 5f); // Set run animation speed
        if (player != null)
        {
            Vector3 directionAway = (transform.position - player.position).normalized;
            // Simple movement: manually move if necessary (or set a destination for the NavMeshAgent)
            transform.position += directionAway * runSpeed * Time.deltaTime;
        }
    }

    // Patrol using the NavMeshAgent.
    public void Patrol()
    {
        anim.SetFloat("Speed", runSpeed); // Set patrol animation speed
        if (agent != null && patrolPoints.Length > 0)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                Debug.Log("Monster patrolling to point: " + currentPatrolIndex);
            }
        }
    }

    // Method to apply damage to the monster.
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Monster takes " + damage + " damage, current HP: " + currentHP);
        if (currentHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        Debug.Log("Monster has died!");
        anim.SetTrigger("Die"); // Trigger death animation
        yield return new WaitForSeconds(2f); // Wait for the death animation to finish
        Destroy(gameObject);
    }

    // Optional: if using triggers for weapon hits.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage(10);  // Example fixed damage if hit by a player's attack collider.
        }
    }
}