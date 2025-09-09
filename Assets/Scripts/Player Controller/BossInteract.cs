using UnityEngine;

public class BossInteract : MonoBehaviour
{
    public GameObject bossHealthBar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            bossHealthBar.SetActive(true);
        }
    }
}
