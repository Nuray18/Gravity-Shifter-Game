using UnityEngine;

public abstract class EnemyHealth : MonoBehaviour
{
    protected float maxHealth = 100f; // bu genel bir deger 
    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void ResetHealth()
    {
        currentHealth = maxHealth; // Canı en yüksek seviyeye çıkar
    }


    protected abstract void Die(); // Alt sınıflar kendi ölüm davranışlarını belirleyecek
}
