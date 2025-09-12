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

    private float lastAttackTime;
    private AttackColliderSet currentAttack;

    private void Start()
    {
        DisableAllColliders();
    }

    // Gọi từ Animation Event khi bắt đầu animation
    public void StartAttack(string animationName)
    {
        // Tìm attack set tương ứng
        currentAttack = attackSets.Find(x => x.animationName == animationName);
        if (currentAttack == null)
        {
            Debug.LogWarning($"Không tìm thấy attack set cho animation: {animationName}");
            return;
        }

        EnableCurrentAttackColliders();
    }

    public void EndAttack()
    {
        DisableAllColliders();
        lastAttackTime = Time.time;
    }


    public void ExecuteRandomAttack()
    {
        // Kiểm tra combo time
        if (Time.time - lastAttackTime > comboResetTime)
        {
            // Reset về attack đầu tiên nếu quá thời gian combo
            currentAttack = attackSets[0];
        }
        else
        {
            // Random attack tiếp theo (có thể thêm logic combo ở đây)
            currentAttack = attackSets[Random.Range(0, attackSets.Count)];
        }

        // Kích hoạt animation - cần setup Animator Controller phù hợp
        GetComponent<Animator>().Play(currentAttack.animationName);
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
