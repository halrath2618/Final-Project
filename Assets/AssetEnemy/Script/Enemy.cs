using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 5f;
    public float patrolWaitTime = 2f;
    private Vector3 patrolCenter;
    private Vector3 patrolTarget;
    private float patrolTimer;

    [Header("Detection Settings")]
    public float detectionRadius = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    private float attackTimer;
    private bool playerDetected;
    private bool canAttack = true;

    [Header("References")]
    public Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator; // ? B?n yêu c?u gi? l?i dòng này

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        patrolCenter = transform.position;
        patrolTarget = GetRandomPatrolPoint();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            Debug.LogError("Animator ch?a ???c gán trong Inspector!", this);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Cooldown attack
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
                canAttack = true;
        }

        // Logic phát hi?n và hành ??ng
        if (distanceToPlayer <= detectionRadius)
        {
            playerDetected = true;
            ChasePlayer();
        }
        else
        {
            playerDetected = false;
            Patrol();
        }

        // T?n công n?u trong t?m
        if (playerDetected && distanceToPlayer <= attackRange && canAttack)
        {
            Attack();
        }

        // C?p nh?t animation
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("Detect", playerDetected);



    }

    void Patrol()
    {
        agent.stoppingDistance = 0.1f;
        agent.speed = 2f;
        agent.SetDestination(patrolTarget);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                patrolTarget = GetRandomPatrolPoint();
                patrolTimer = 0f;
            }
        }
    }

    void ChasePlayer()
    {
        agent.stoppingDistance = attackRange;
        agent.speed = 3.5f;
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        canAttack = false;
        attackTimer = attackCooldown;

        agent.ResetPath(); // D?ng l?i khi t?n công
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        animator.SetTrigger("Attack");

        // Gây damage tr?c ti?p (n?u có PlayerController)
        if (player.TryGetComponent(out PlayerController playerCtrl))
        {
            playerCtrl.TakeDamage(10);
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector2 random = Random.insideUnitCircle * patrolRadius;
        Vector3 randomPoint = patrolCenter + new Vector3(random.x, 0, random.y);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return patrolCenter;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
