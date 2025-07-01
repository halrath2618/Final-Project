using UnityEngine;

public class MonsterClaw : MonoBehaviour
{
    private Monster monster;
    [SerializeField] private PlayerController playerController;

    void Start()
    {
        // Get reference to parent monster
        monster = GetComponentInParent<Monster>();

        // Ensure collider is trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Only trigger attack if colliding with player
        if (other.CompareTag("Player"))
        {
            playerController.TakeDamage(monster.attackDamage);
        }
    }
}