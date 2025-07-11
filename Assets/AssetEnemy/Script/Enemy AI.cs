using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 2f;
    public float attackDistance = 1f;
    public float patrolRadius = 1f;

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 startPoint;
    private Vector3 patrolTarget;

    private bool isChasing = false;
    private bool isReturning = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startPoint = transform.position;
        ChooseNewPatrolPoint();
    }

    void Update()
    {
        float distToPlayerFromStart = Vector3.Distance(player.position, startPoint);
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        // Xác ??nh tr?ng thái
        if (distToPlayerFromStart <= detectionRange)
        {
            isChasing = true;
            isReturning = false;
        }
        else if (isChasing && distToPlayerFromStart > detectionRange)
        {
            isChasing = false;
            isReturning = true;
        }

        // Hành vi
        if (isChasing)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
        }
        else if (isReturning)
        {
            agent.SetDestination(startPoint);
            animator.SetBool("isRunning", true);

            if (Vector3.Distance(transform.position, startPoint) < 0.2f)
            {
                isReturning = false;
                ChooseNewPatrolPoint();
            }
        }
        else
        {
            Patrol();
        }

        // T?n công n?u ?? g?n
        if (distToPlayer <= attackDistance)
        {
            Attack();
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 0.3f)
        {
            ChooseNewPatrolPoint();
        }

        agent.SetDestination(patrolTarget);
        animator.SetBool("isRunning", true);
    }

    void ChooseNewPatrolPoint()
    {
        Vector2 offset = Random.insideUnitCircle * patrolRadius;
        patrolTarget = startPoint + new Vector3(offset.x, 0, offset.y);
    }

    void Attack()
    {
        agent.ResetPath();
        animator.SetBool("isAttacking", true);
        animator.SetBool("isRunning", false);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        // Thêm x? lý gi?m máu Player ? ?ây n?u mu?n
    }
}
