using UnityEngine;

public class EnemyProjecTile : MonoBehaviour
{
    private float damage;
    private float lifetime = 5f;

    public void SetDamage(float dmg) => damage = dmg;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
