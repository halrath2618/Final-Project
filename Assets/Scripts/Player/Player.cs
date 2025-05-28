using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP = 100;

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Player takes " + damage + " damage, current HP: " + currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implement your game-over or respawn logic here.
    }

    // If you want to use collider check for when the monster’s attack hits,
    // you could implement an OnTriggerEnter as shown below. (Make sure your colliders and tags are set appropriately.)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            // This branch would be used if your monster had a dedicated attack collider.
            TakeDamage(10);
        }
    }
}