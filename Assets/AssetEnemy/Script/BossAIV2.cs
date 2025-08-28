using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public interface IDamageable
{
    void TakeDamage(float amount);
}

[RequireComponent(typeof(NavMeshAgent))]
public class BossAIV2 : MonoBehaviour
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
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public LayerMask playerLayer;

    private float lastAttackTime;
    private bool isPlayerInSight;
    private float strafeTimer;
    private float directionChangeTimer;
    private int strafeDirection = 1;
    private Vector3 strafeTargetPosition;
    private bool isActionComplete = true;
    private AIState nextStateAfterAction = AIState.Idle;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        lastAttackTime = -attackCooldown;
        isActionComplete = true;
    }

    void Update()
    {
        CheckPlayerVisibility();

        if (isActionComplete)
        {
            ProcessStateDecision();
        }
        else
        {
            ProcessCurrentAction();
        }

        UpdateAnimator();
    }

    void ProcessStateDecision()
    {
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
                AttackingDecision();
                break;
            case AIState.Strafe:
                StrafeDecision();
                break;
        }
    }

    void ProcessCurrentAction()
    {
        switch (currentState)
        {
            case AIState.Attacking:
                ProcessAttackAction();
                break;
            case AIState.Strafe:
                ProcessStrafeAction();
                break;
        }
    }

    void CheckPlayerVisibility()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isPlayerInSight = false;

        if (distanceToPlayer <= detectionRadius)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, playerLayer))
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
            DecideNextAction();
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
            DecideNextAction();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
        FaceTarget(player.position);
    }

    void DecideNextAction()
    {
        if (UnityEngine.Random.value < strafeProbability)
        {
            PrepareStrafeAction();
        }
        else
        {
            PrepareAttackAction();
        }
    }

    void AttackingDecision()
    {
        if (!isPlayerInSight)
        {
            CompleteAction(AIState.Idle);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange * 1.2f)
        {
            CompleteAction(AIState.Chasing);
            return;
        }

        PrepareAttackAction();
    }

    void StrafeDecision()
    {
        if (!isPlayerInSight)
        {
            CompleteAction(AIState.Idle);
            return;
        }

        PrepareStrafeAction();
    }

    void PrepareStrafeAction()
    {
        isActionComplete = false;
        currentState = AIState.Strafe;
        StartStrafe();
    }

    void PrepareAttackAction()
    {
        isActionComplete = false;
        currentState = AIState.Attacking;
        agent.isStopped = true;
        nextStateAfterAction = AIState.Finding;
    }

    void StartStrafe()
    {
        strafeTimer = 0f;
        directionChangeTimer = 0f;
        strafeDirection = UnityEngine.Random.value < 0.5f ? 1 : -1;
        CalculateStrafePosition();
    }

    void CalculateStrafePosition()
    {
        if (player == null) return;

        Vector3 playerToBoss = transform.position - player.position;
        playerToBoss.y = 0;
        playerToBoss.Normalize();

        Vector3 strafeDirectionVector = Quaternion.Euler(0, 90 * strafeDirection, 0) * playerToBoss;
        strafeTargetPosition = player.position + strafeDirectionVector * strafeDistance;

        agent.isStopped = false;
        agent.SetDestination(strafeTargetPosition);
    }

    void ProcessStrafeAction()
    {
        if (player == null || !isPlayerInSight)
        {
            CompleteAction(AIState.Idle);
            return;
        }

        strafeTimer += Time.deltaTime;
        directionChangeTimer += Time.deltaTime;

        // Đổi hướng strafe định kỳ
        if (directionChangeTimer >= strafeChangeDirectionTime)
        {
            directionChangeTimer = 0f;
            strafeDirection *= -1;
            CalculateStrafePosition();
        }

        FaceTarget(player.position);

        // Kiểm tra hoàn thành strafe
        float distanceToStrafeTarget = Vector3.Distance(transform.position, strafeTargetPosition);
        if (distanceToStrafeTarget < 1f || strafeTimer >= strafeDuration)
        {
            CompleteAction(AIState.Attacking);
        }
    }

    void ProcessAttackAction()
    {
        if (player == null || !isPlayerInSight)
        {
            CompleteAction(AIState.Idle);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange * 1.2f)
        {
            CompleteAction(AIState.Chasing);
            return;
        }

        FaceTarget(player.position);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
            CompleteAction(AIState.Finding);
        }
    }

    void Attack()
    {
        int rand = Random.Range(1, 3);
        animator.SetInteger("AttackType", rand);
        animator.SetTrigger("Attack");

        //// Thêm logic damage tại đây
        //// Ví dụ: Raycast để kiểm tra trúng player
        //RaycastHit hit;
        //Vector3 attackDirection = (player.position - transform.position).normalized;
        //if (Physics.Raycast(transform.position, attackDirection, out hit, attackRange, playerLayer))
        //{
        //    if (hit.transform == player)
        //    {
        //        Debug.Log("Player hit!");
        //        // Gây damage cho player
        //    }
        //}
    }

    void CompleteAction(AIState nextState)
    {
        isActionComplete = true;
        nextStateAfterAction = nextState;
        currentState = nextStateAfterAction;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude / agent.speed);
        animator.SetBool("IsStrafing", currentState == AIState.Strafe);

        // Cập nhật trạng thái tấn công
        animator.SetBool("IsAttacking", currentState == AIState.Attacking);
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi phát hiện
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Vẽ phạm vi tấn công
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Vẽ hướng strafe
        if (currentState == AIState.Strafe)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, strafeTargetPosition);
            Gizmos.DrawSphere(strafeTargetPosition, 0.5f);
        }
    }
}
