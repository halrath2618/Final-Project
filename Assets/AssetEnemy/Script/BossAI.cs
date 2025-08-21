using System;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Finding,
        Chasing,
        Attack,
        Strafe
    }

    [Header("AI Settings")]
    public AIState currentState = AIState.Idle;
    [SerializeField] private float detection = 15f;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private float strafeDistance = 5f; //Khoảng cách từ player khi strafe
    [SerializeField] private float strafeDuration = 2f; //Thời gian tối đa strafe
    [SerializeField] private float strafeChangeDirectionTime =.5f; //Tần suất đổi hướng
    [Range(0, 1)] public float strafeProbability = .5f; //Xác suất chọn strafe thay vì tấn công

    [Header("References")]
    private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask playerLayer;


    private float lastAttackTime;
    private bool isPlayerInSight;
    private float strafeTimer;
    private float directionChangeTimer;
    private int strafeDirection = 1;
    private Vector3 strafeTargetPosition;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;

        }
        lastAttackTime = -attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();

        switch (currentState) {
            case AIState.Idle:
                IdleState();
                break;
            case AIState.Finding:
                FindingState();
                break;
            case AIState.Chasing:
                ChasingState();
                break;
            case AIState.Attack:
                AttackingState(); 
                break;
            case AIState.Strafe:
                StrafeState();
                break;
        }
    }

    private void DetectPlayer()
    {

        Vector3 distanceToPlayer =player.position - transform.position;
        isPlayerInSight = false;
        float distance = distanceToPlayer.magnitude;
        if (distance < detection)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, distanceToPlayer.normalized, out hit)) { 
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
            currentState = AIState.Attack;
        }
        else if (distanceToPlayer <= detection)
        {
            currentState = AIState.Chasing;
            animator.SetFloat("walk", agent.velocity.magnitude);

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
            currentState = AIState.Attack;
            agent.isStopped = true;
            return;
        }
        
        agent.isStopped = false;    
        agent.SetDestination(player.position);
        FaceTarget(player.position);
    }

    void AttackingState()
    {
        if (!isPlayerInSight)
        {
            currentState = AIState.Idle;
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange *1.2f)
        {
            currentState = AIState.Chasing;
            return;
        }
        FaceTarget(player.position);
        if(Time.time -lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;

            if (UnityEngine.Random.value < strafeProbability * 0.5f)
            {
                StartStrafe();
            }
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    #region Strafe
    void StartStrafe()
    {
        currentState = AIState.Strafe;
        strafeTimer = 0f;
        directionChangeTimer = 0f;
        strafeDirection = UnityEngine.Random.value < 0.5f ? 1 : -1;
        CalculateStrafePosition();
    }

    private void CalculateStrafePosition()
    {
        Vector3 playerToBoss = transform.position - player.position;
        playerToBoss.y = 0;
        playerToBoss.Normalize();

        Vector3 strafeDirectionVector = Quaternion.Euler(0, 90 * strafeDirection, 0) * playerToBoss;
        strafeTargetPosition = player.position + strafeDirectionVector * strafeDistance;

        agent.isStopped =false;
        agent.SetDestination(strafeTargetPosition);
    
    }
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
            strafeDirection *= -1;// Reverse direction
            CalculateStrafePosition();
        }

        float distanceToStrafeTarget = Vector3.Distance(transform.position, strafeTargetPosition);

        if (distanceToStrafeTarget < 1f || strafeTimer >= strafeDuration)
        {
            currentState = AIState.Attack;
            agent.isStopped =  true;
            return;
        }
        FaceTarget(player.position);
    }

    #endregion


    private void FaceTarget(Vector3 target)
    {
        var direction = (target - transform.position).normalized;
        Quaternion lookRation = Quaternion.LookRotation(new Vector3(direction.x,0, direction.z
            ));
        transform.rotation= Quaternion.Slerp(transform.rotation,lookRation,Time.deltaTime * rotationSpeed);
    }
    void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi phát hiện
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detection);

        // Vẽ phạm vi tấn công
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
