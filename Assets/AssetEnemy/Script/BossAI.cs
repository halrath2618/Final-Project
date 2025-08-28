using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Finding,
        Chasing,
        Attacking,
        Strafe
    }

    [Header("AI Settings")]
    public AIState currentState = AIState.Idle;
    public float detectionRadius = 15f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public float rotationSpeed = 5f;
    public float strafeDistance = 5f;
    public float strafeDuration = 2f;
    public float strafeChangeDirectionTime = 0.5f;
    [Range(0, 1)] public float strafeProbability = 0.5f;

    [Header("References")]
    private Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public LayerMask playerLayer;

    private float lastAttackTime;
    private bool isPlayerInSight;
    private float strafeTimer;
    private float directionChangeTimer;
    private int strafeDirection = 1; // 1 for right, -1 for left
    private Vector3 strafeTargetPosition;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        CheckPlayerVisibility();

        switch (currentState)
        {
            case AIState.Idle:
                IdleState();
                break;
            case AIState.Finding:
                FindingState();
                break;
            case AIState.Chasing:
                ChasingState();
                break;
            case AIState.Attacking:
                AttackingState();
                break;
            case AIState.Strafe:
                StrafeState();
                break;
        }

        //UpdateAnimator();
    }

    void CheckPlayerVisibility()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isPlayerInSight = false;

        if (distanceToPlayer <= detectionRadius)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
            {
                if (hit.transform == player)
                {
                    isPlayerInSight = true;
                }
            }
        }
    }

    void IdleState()
    {
        agent.isStopped = true;

        if (isPlayerInSight)
        {
            currentState = AIState.Finding;
        }
    }

    void FindingState()
    {
        if (!isPlayerInSight)
        {
            currentState = AIState.Idle;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            DecideAttackOrStrafe();
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            currentState = AIState.Chasing;
        }
    }

    void ChasingState()
    {
        if (!isPlayerInSight)
        {
            currentState = AIState.Idle;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            DecideAttackOrStrafe();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
        FaceTarget(player.position);
    }

    void DecideAttackOrStrafe()
    {
        // Sử dụng UnityEngine.Random thay vì Random
        if (UnityEngine.Random.value < strafeProbability && agent.remainingDistance <= attackRange * 1.5f)
        {
            StartStrafe();
        }
        else
        {
            currentState = AIState.Attacking;
            agent.isStopped = true;
        }
    }
    #region Strafe
    void StartStrafe()
    {
        currentState = AIState.Strafe;
        strafeTimer = 0f;
        directionChangeTimer = 0f;
        // Sửa thành UnityEngine.Random
        strafeDirection = UnityEngine.Random.value < 0.5f ? 1 : -1; // Random left or right
        CalculateStrafePosition();
    }

    void CalculateStrafePosition()
    {
        Vector3 playerToBoss = transform.position - player.position;
        playerToBoss.y = 0;
        playerToBoss.Normalize();

        // Get perpendicular vector for strafing (90 degrees)
        Vector3 strafeDirectionVector = Quaternion.Euler(0, 90 * strafeDirection, 0) * playerToBoss;
        strafeTargetPosition = player.position + strafeDirectionVector * strafeDistance;

        agent.isStopped = false;
        agent.SetDestination(strafeTargetPosition);
    }
    #endregion
    void StrafeState()
    {
        if (!isPlayerInSight)
        {
            currentState = AIState.Idle;
            return;
        }

        strafeTimer += Time.deltaTime;
        directionChangeTimer += Time.deltaTime;

        // Change strafe direction periodically
        if (directionChangeTimer >= strafeChangeDirectionTime)
        {
            directionChangeTimer = 0f;
            strafeDirection *= -1; // Reverse direction
            CalculateStrafePosition();
        }

        // Check distance to strafe target
        float distanceToStrafeTarget = Vector3.Distance(transform.position, strafeTargetPosition);

        if (distanceToStrafeTarget < 1f || strafeTimer >= strafeDuration)
        {
            // When done strafing, go to attack
            currentState = AIState.Attacking;
            agent.isStopped = true;
            return;
        }

        FaceTarget(player.position); // Keep facing player while strafing
    }

    void AttackingState()
    {
        if (!isPlayerInSight)
        {
            currentState = AIState.Idle;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange * 1.2f)
        {
            currentState = AIState.Chasing;
            return;
        }

        FaceTarget(player.position);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;

            // After attacking, decide to strafe again or chase
            // Sử dụng UnityEngine.Random
            if (UnityEngine.Random.value < strafeProbability * 0.5f) // Lower probability to prevent infinite strafe
            {
                StartStrafe();
            }
        }
    }

    void Attack()
    {
        int rand = Random.Range(1, 3);
        animator.SetInteger("AttackType", rand);
        animator.SetTrigger("Attack");
        Debug.Log("Boss attacking player!");
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void UpdateAnimator()
    {
        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude / agent.speed);
        animator.SetBool("IsStrafing", currentState == AIState.Strafe);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (currentState == AIState.Strafe)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, strafeTargetPosition);
            Gizmos.DrawSphere(strafeTargetPosition, 0.5f);
        }
    }
}