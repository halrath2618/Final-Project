using System.Collections;
using UnityEngine;
using static AttackAnimationManager;

public class EnemyCombatAnimation : MonoBehaviour
{
    public Animator animator;
    public Collider[] attackColliders;
    public float attackCooldown = 2f;
    private float cooldownTimer;
    [SerializeField] private int maxAttackTypes = 3;
    private bool inCombat = false;

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

}
