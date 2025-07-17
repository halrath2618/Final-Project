using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : MonoBehaviour
{
    [System.Serializable]
    public class AttackColliderSet
    {
        public string animationName; // Tên animation
        public Collider[] attackColliders; 
        public int damage; // Sát thương của đòn tấn công 
    }

    [SerializeField] private List<AttackColliderSet> attackSets = new List<AttackColliderSet>();
    [SerializeField] private float comboResetTime = 2f;

    [Header("Layer Settings")]
    [SerializeField] private int combatLayerIndex = 1; // Layer chiến đấu
    [SerializeField] private float layerActivationSpeed = 5f;

    private Animator animator;
    private float lastAttackTime;
    private AttackColliderSet currentAttack;
    private Dictionary<int, AttackColliderSet> attackDictionary;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        attackDictionary = new Dictionary<int, AttackColliderSet>();
        for (int i = 0; i < attackSets.Count; i++)
        {
            attackDictionary.Add(i, attackSets[i]);
        }
        DisableAllColliders();
    }

    // Gọi từ Animation Event khi bắt đầu animation
    public void StartAttack(int attackIndex)
    {
        // Tìm attack set tương ứng
        if (attackDictionary.TryGetValue(attackIndex, out currentAttack))
        {
            EnableCurrentAttackColliders();
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy attack set với index: {attackIndex}");
        }
    }

    public void EndAttack()
    {
        DisableAllColliders();
        lastAttackTime = Time.time;
    }


    public void ExecuteRandomAttack(int attackIndex)
    {
        if (attackSets.Count == 0) return;

        if (Time.time - lastAttackTime > comboResetTime)
        {
            attackIndex = 0; // Reset về attack đầu tiên nếu quá thời gian combo
        }

        // Đảm bảo index hợp lệ
        if (attackIndex >= 0 && attackIndex < attackSets.Count)
        {
            currentAttack = attackSets[attackIndex];
            animator.SetInteger("AttackType", attackIndex);
            animator.SetTrigger("Attack");
        }
    }

    private void EnableCurrentAttackColliders()
    {
        if (currentAttack == null) return;

        foreach (var collider in currentAttack.attackColliders)
        {
            collider.enabled = true;
            collider.isTrigger = true;
        }
    }

    private void DisableAllColliders()
    {
        foreach (var attackSet in attackSets)
        {
            foreach (var collider in attackSet.attackColliders)
            {
                collider.enabled = false;
                collider.isTrigger = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentAttack != null)
        {
            Debug.Log($"Player take {currentAttack.damage} damage {currentAttack.animationName}");
            // Gọi hàm nhận sát thương của player ở đây
            // other.GetComponent<PlayerHealth>().TakeDamage(currentAttack.damage);
        }
    }
}
