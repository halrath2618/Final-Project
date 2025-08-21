using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 15f;
   

    public void Shooting()
    {
        // Animation event sẽ gọi hàm này hoặc gọi trực tiếp
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (transform.forward + Vector3.up * 0.1f).normalized; // Góc bắn hơi chếch lên
            rb.linearVelocity = direction * projectileSpeed;
        }

       
    }
}
