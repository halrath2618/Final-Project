using System.Collections;
using UnityEngine;
using static AttackAnimationManager;

public class EnemyCombatAnimation : MonoBehaviour
{
    [SerializeField]private Animator animator;
    public Collider[] attackColliders;
    public float attackCooldown = 2f;
    private float cooldownTimer;
    [SerializeField] private int maxAttackTypes = 3;

    [Header("Attack Damage")]
    [SerializeField] private float[] attackDamage;
    [SerializeField] private LayerMask nameLayer;
    private bool hasDealtDamage = false;

    void Start()
    {
        cooldownTimer = 0f;
        DisableAttackColliders();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public bool CanAttack()
    {
        return cooldownTimer <= 0f ;
    }

    public void TriggerRandomAttack()
    {
        int rand = Random.Range(1, maxExclusive: maxAttackTypes+1);
        animator.SetInteger("AttackType", rand);
        animator.SetTrigger("Attack");
        cooldownTimer = attackCooldown;
    }

    // Called from Animation Event
    public void EnableAttackCollider(int index)
    {
         for (int i = 0; i < attackColliders.Length; i++)
        {
            attackColliders[i].enabled = (i == index);
           // Debug.Log($"Collider {i} ({attackColliders[i].name}) => {(i == index ? "ENABLED" : "disabled")}");
        }
    }
    public void DisableAttackColliders()
    {
        foreach (var col in attackColliders)
        {
            col.enabled = false;
        }
    }

    public void DealDamage(int attackIndex)
    {
         int damageIndex = attackIndex;
    
        if (damageIndex >= 0 && damageIndex < attackDamage.Length)
        {
            // Tìm player gần nhất trong phạm vi
            Collider[] hitPlayers = Physics.OverlapSphere(transform.position, 2f, nameLayer);
        
            foreach (var player in hitPlayers)
            {
                PlayerController playerHealth = player.GetComponent<PlayerController>();
                if (playerHealth != null)
                {
                    hasDealtDamage = true;
                    //playerHealth.TakeDamage(attackDamage[damageIndex]);
                    Debug.Log($"Dealt {attackDamage[damageIndex]} damage with attack {attackIndex} to {player.name}");

                    Invoke(nameof(ResetDealDamage), 2f);
                }
            }
        }
    }


    private void ResetDealDamage()
    {
        hasDealtDamage = false;
    }
}
