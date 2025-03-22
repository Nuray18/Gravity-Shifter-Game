using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEaterHealth : MonoBehaviour, IHealth
{
    private float currentHealth;
    [SerializeField]
    private float maxHealth = 40;

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
        GetComponent<GravityEaterAI>()?.Die();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; // Canı en yüksek seviyeye çıkar
    }
}
