using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    private float currentHealth;
    [SerializeField]
    private float maxHealth = 30;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<EnemyGravityEater>()?.Die();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; // Canı en yüksek seviyeye çıkar
    }
}
