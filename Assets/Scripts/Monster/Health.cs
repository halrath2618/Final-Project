// Health system for monsters
using UnityEngine.AI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private MonsterUI monsterUI; // Assuming you have a UI script to update health display
    [SerializeField] private PlayerController playerController;
    void Start()
    {
        currentHealth = maxHealth;
    }



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        monsterUI.UpdateHealthBar(); // Update the health bar UI
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Ensure health doesn't go below zero
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