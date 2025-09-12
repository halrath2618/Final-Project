using UnityEngine;
using UnityEngine.AI;

public class NPCPatrolling : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float patrolSpeed = 2f;
    public float patrolWaitTime = 3f;
    private Vector3 startPosition;
    private Vector3 patrolTarget;
    private float patrolTimer;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startPosition == Vector3.zero)
        {
            startPosition = transform.position;
            patrolTarget = GetRandomPatrolPoint();
            agent.speed = patrolSpeed;
        }
        PatrolBehavior();
    }

    void PatrolBehavior()
    {
        // Set animations
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetFloat("Speed", 5);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        // Move to patrol point
        agent.SetDestination(patrolTarget);

        // Check if reached destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            animator.SetFloat("Speed", 0);

            // Wait at point before moving to next
            if (patrolTimer >= patrolWaitTime)
            {
                patrolTarget = GetRandomPatrolPoint();
                patrolTimer = 0;
            }
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
}
