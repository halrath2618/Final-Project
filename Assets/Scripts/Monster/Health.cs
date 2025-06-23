// Health system for monsters
using UnityEngine.AI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool IsDead => currentHealth <= 0;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    [SerializeField] private Animator animator;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;

        if (IsDead)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle death - play animation, disable components, etc.
        GetComponent<Monster>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Destroy after delay
        Destroy(gameObject, 5f);
    }
}