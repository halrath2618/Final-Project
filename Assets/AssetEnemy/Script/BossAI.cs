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
    [SerializeField] private float strafeDistance = 5f;
    [SerializeField] private float strafeDuration = 2f;
    [SerializeField] private float strafeChangeDirectionTime =.5f;
    [Range(0, 1)] public float strafeProbability = .5f;

    [Header("References")]
    private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    private float lastAttackTime;
    private bool isPlayerInSight;
    private float strafeTimer;
    private float directionChangeTimer;
    private int strafeDirection = 1;
    private Vector3 strafeTargerPostion;

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
                AttackingState();
                break;
        }
    }

    private void DetectPlayer()
    {

        Vector3 toPlayer =player.position - transform.position;
        isPlayerInSight = false;
        float distance = toPlayer.magnitude;
        if (distance < detection)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, toPlayer.normalized, out hit)) { 
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
        strafeTargerPostion = player.position + strafeDirectionVector * strafeDistance;

        agent.isStopped =false;
        agent.SetDestination(strafeTargerPostion);
    
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

        if (directionChangeTimer >= strafeChangeDirectionTime)
        {
            directionChangeTimer = 0f;
            strafeDirection *= -1;// Reverse direction
            CalculateStrafePosition();
        }
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
    }
}
