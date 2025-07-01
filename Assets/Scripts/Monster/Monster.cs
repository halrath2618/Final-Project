using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float patrolSpeed = 2f;
    public float patrolWaitTime = 3f;
    private Vector3 startPosition;
    private Vector3 patrolTarget;
    private float patrolTimer;

    [Header("Detection Settings")]
    public float detectionDistance = 4f;
    public float detectionAngle = 90f; // 90 degrees in front
    private bool playerDetected;
    [SerializeField] private Transform playerPosition;

    [Header("Combat Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float attackDamage = 10;
    private float attackTimer;
    private bool canAttack = true;

    [Header("Flee Settings")]
    public float fleeHealthThreshold = 0.2f; // 20% health
    public float fleeDistance = 8f;
    public float fleeSpeed = 5f;
    private bool isFleeing;

    [Header("References")]
    public Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;
    [SerializeField] private MonsterUI monsterUI; // Assuming you have a UI script to update health display
    [SerializeField] private PlayerController playerController;
    
    [Header("Attack Colliders")]
    public BoxCollider monsterClaw1;
    public BoxCollider monsterClaw2;
    private bool _isAttacking;
    private bool _attackTriggered;

    [Header("Animation Parameters")]
    public string walkParam = "IsWalking";
    public string runParam = "IsRunning";
    public string attackParam = "Attack";
    public string fleeParam = "Flee";

    void Start()
    {
        monsterClaw1.enabled = false; // Disable colliders initially
        monsterClaw2.enabled = false;
        startPosition = transform.position;
        patrolTarget = GetRandomPatrolPoint();
        agent.speed = patrolSpeed;

        // Find player if not assigned
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        monsterUI.UpdateHealthBar();

        // Check health for flee state
        if (!isFleeing && health.currentHealth <= health.maxHealth * fleeHealthThreshold)
        {
            StartFleeing();
        }

        // Handle attack cooldown
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                canAttack = true;
            }
        }

        // State machine
        if (isFleeing)
        {
            FleeBehavior();
        }
        else if (playerDetected)
        {
            ChaseBehavior();
        }
        else
        {
            PatrolBehavior();
        }

        // Always check for player detection
        CheckPlayerDetection();

        // Disable attack colliders when not attacking
        if (!_isAttacking)
        {
            monsterClaw1.enabled = false;
            monsterClaw2.enabled = false;
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        Vector3 randomPoint = startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

        // Ensure point is on NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return startPosition;
    }

    void PatrolBehavior()
    {
        // Set animations
        animator.SetBool(walkParam, true);
        animator.SetBool(runParam, false);

        // Move to patrol point
        agent.SetDestination(patrolTarget);

        // Check if reached destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            animator.SetBool(walkParam, false);

            // Wait at point before moving to next
            if (patrolTimer >= patrolWaitTime)
            {
                patrolTarget = GetRandomPatrolPoint();
                patrolTimer = 0;
            }
        }
    }

    void CheckPlayerDetection()
    {
        if (player == null || isFleeing) return;

        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        // Check if player is in detection range
        if (distance <= detectionDistance)
        {
            // Check if player is in front (within detection angle)
            float angle = Vector3.Angle(transform.forward, toPlayer);
            if (angle <= detectionAngle / 2)
            {
                // Raycast to ensure line of sight
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, toPlayer.normalized, out hit, detectionDistance))
                {
                    if (hit.transform == player)
                    {
                        playerDetected = true;
                        return;
                    }
                }
            }
        }

        playerDetected = false;
    }

    void ChaseBehavior()
    {
        // Set animations
        animator.SetBool(walkParam, false);
        animator.SetBool(runParam, true);

        // Move toward player
        agent.SetDestination(playerPosition.position);
        if (Vector3.Distance(transform.position, playerPosition.position) <= attackRange && canAttack)
        {
            AttackPlayer();
        }
        else if (Vector3.Distance(transform.position, playerPosition.position) > attackRange)
        {
            // Continue chasing if not in attack range
            agent.isStopped = false;
            _isAttacking = false;
            monsterClaw1.enabled = false;
            monsterClaw2.enabled = false;
        }

        // Face player while chasing
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    void AttackPlayer()
    {
        animator.SetBool(walkParam, false);
        animator.SetBool(runParam, false);
        // Set attack state
        _isAttacking = true;
        canAttack = false;
        attackTimer = attackCooldown;

        // Stop movement during attack
        agent.isStopped = true;

        // Trigger attack animation

        animator.SetTrigger(attackParam);

        // Enable colliders for attack
        monsterClaw1.enabled = true;
        monsterClaw2.enabled = true;
    }
    public void FinishAttack()
    {
        // Reset attack state
        _isAttacking = false;
        agent.isStopped = false;

        // Disable attack colliders
        monsterClaw1.enabled = false;
        monsterClaw2.enabled = false;
    }

    // Called from attack animation event

    void StartFleeing()
    {
        isFleeing = true;
        playerDetected = false;
        agent.speed = fleeSpeed;
        animator.SetBool(fleeParam, true);
        animator.SetBool(runParam, false);
        animator.SetBool(walkParam, false);
    }

    void FleeBehavior()
    {
        // Flee away from player
        if (player != null)
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

            // Ensure point is on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        // Check if should stop fleeing (optional)
        if (Vector3.Distance(transform.position, player.position) > fleeDistance * 1.5f)
        {
            StopFleeing();
        }
    }

    // Optional method to stop fleeing
    void StopFleeing()
    {
        isFleeing = false;
        agent.speed = patrolSpeed;
        animator.SetBool(fleeParam, false);
        patrolTarget = GetRandomPatrolPoint();
    }

    void OnDrawGizmosSelected()
    {
        // Draw patrol radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);

        // Draw detection range
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionDistance);

            // Draw detection cone
            Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionDistance;

            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
            Gizmos.DrawLine(transform.position + leftBoundary, transform.position + transform.forward * detectionDistance);
            Gizmos.DrawLine(transform.position + rightBoundary, transform.position + transform.forward * detectionDistance);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill_E"))
        {
            health.TakeDamage(playerController.attackDmg_Earth);
            Debug.Log("Monster hit by Earth skill, current health: " + health.currentHealth);
        }
        else if (other.CompareTag("Skill_F"))
        {
            health.TakeDamage(playerController.attackDmg_Fire);
            Debug.Log("Monster hit by Fire skill, current health: " + health.currentHealth);
        }
    }
}