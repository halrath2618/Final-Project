using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 1f;
    public float patrolWaitTime = 2f;
    private Vector3 startPosition;
    private Vector3 patrolTarget;
    private float patrolTimer;

    [Header("Detection & Combat")]
    public float detectionDistance = 2f;
    public float attackDistance = 1f;
    public float attackCooldown = 2f;
    private float attackTimer;
    private bool canAttack = true;
    private bool playerDetected = false;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    [Header("Animator Parameters")]
    public string walkParam = "IsWalking";
    public string runParam = "IsRunning";
    public string attackParam = "Attack";

    void Start()
    {
        startPosition = transform.position;
        patrolTarget = GetRandomPatrolPoint();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
                canAttack = true;
        }

        if (player == null) return;

        float distToPlayerFromStart = Vector3.Distance(player.position, startPosition);
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        playerDetected = distToPlayerFromStart <= detectionDistance;

        if (playerDetected)
        {
            ChasePlayer();

            if (distToPlayer <= attackDistance && canAttack)
            {
                Attack();
            }
        }
        else
        {
            PatrolBehavior();
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector2 offset = Random.insideUnitCircle * patrolRadius;
        Vector3 point = startPosition + new Vector3(offset.x, 0, offset.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, patrolRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return startPosition;
    }

    void PatrolBehavior()
    {
        animator.SetBool(walkParam, true);
        animator.SetBool(runParam, false);

        agent.speed = 1.5f;
        agent.SetDestination(patrolTarget);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            animator.SetBool(walkParam, false);

            if (patrolTimer >= patrolWaitTime)
            {
                patrolTarget = GetRandomPatrolPoint();
                patrolTimer = 0;
            }
        }
    }

    void ChasePlayer()
    {
        animator.SetBool(walkParam, false);
        animator.SetBool(runParam, true);

        agent.speed = 3f;
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        canAttack = false;
        attackTimer = attackCooldown;

        agent.ResetPath();
        animator.SetBool(walkParam, false);
        animator.SetBool(runParam, false);
        animator.SetTrigger(attackParam);

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    // G?i t? Animation Event n?u c?n
    public void DealDamage()
    {
        Debug.Log("Enemy attacked the player!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition == Vector3.zero ? transform.position : startPosition, detectionDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(startPosition == Vector3.zero ? transform.position : startPosition, patrolRadius);
    }
}
