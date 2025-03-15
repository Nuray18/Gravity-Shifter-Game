using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private float damage = 5f; // enemy'nin verdigi damage

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
