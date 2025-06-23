// Health system for monsters
using UnityEngine.AI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public bool IsDead => currentHealth <= 0;
    [SerializeField] private Animator animator;
    [SerializeField] private MonsterUI monsterUI; // Assuming you have a UI script to update health display
    [SerializeField] private PlayerController playerController;
    void Start()
    {
        currentHealth = maxHealth;
    }



    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        monsterUI.UpdateHealthBar();
        if (IsDead)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<Monster>().enabled = false; // Disable monster behavior script
                                                 // Trigger death animation
        animator.SetTrigger("Die");


        // Destroy after delay
        Destroy(gameObject, 5f);
    }
}