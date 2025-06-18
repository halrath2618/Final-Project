using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float currentHP = 100f;
    public Slider healthBar;  // Assign your health bar slider in the Inspector

    [Header("Damage Settings")]
    public int normalAttackDamage = 10;
    public int strongAttackDamage = 20;

    [Header("Attack Settings")]
    public float attackRange = 0.1f;  // Radius for collider check on attack

    [Header("Movement Settings")]
    public float runSpeed;
    public Transform player;  // Assign your Player transform in the Inspector
    [SerializeField] private Animator anim;  //Animator for monster animations

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public NavMeshAgent agent;

    private BehaviorTree behaviorTree;


    public PlayerController playerController;
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
        HPUpdate(); // Update the health bar UI
        // Evaluate behavior tree every frame
        behaviorTree.Evaluate();
    }

    // ---------- Action Methods ----------

    public void Attack()
    {
        Debug.Log("Monster attacks the player!");
        anim.SetTrigger("Attack"); // Trigger attack animation
        if (player != null)
        {
            // Check if player is within attack range
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                // Apply damage to the player (assuming player has a TakeDamage method)
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(normalAttackDamage);
                    Debug.Log("Player takes " + normalAttackDamage + " damage.");
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
    public void TakeDamage(float damage)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill_E"))
        {
            TakeDamage(playerController.attackDmg_Earth); // Assuming the skill has a damage value
        }
        else if (other.CompareTag("Skill_F"))
        {
            TakeDamage(playerController.attackDmg_Fire); // Assuming the skill has a damage value
        }
    }

    private void HPUpdate()
    {
        healthBar.maxValue = maxHP; // Set the maximum value of the health bar
        healthBar.value = currentHP; // Update the health bar value
    }
}