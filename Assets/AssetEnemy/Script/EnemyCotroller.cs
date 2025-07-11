using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    private int currentPatrolIndex = 0;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // C?p nh?t thông s? Speed cho Animator
        animator.SetFloat("Speed", agent.velocity.magnitude);
        animator.SetFloat("Run", agent.velocity.magnitude);
    }

    void Patrol()
    {
        animator.SetBool("Detect", false);

        agent.speed = patrolSpeed;
        agent.destination = patrolPoints[currentPatrolIndex].position;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("Detect", true);
        agent.speed = chaseSpeed;
        agent.destination = player.position;
    }

    void AttackPlayer()
    {
        agent.ResetPath();
        transform.LookAt(player);

        animator.SetBool("Detect", true);
        animator.SetTrigger("Attack");
    }

    public void Die()
    {
        isDead = true;
        agent.enabled = false;
        animator.SetBool("isDead", true);
    }

    public void TakeHit()
    {
        animator.SetTrigger("isHit");
    }
}
