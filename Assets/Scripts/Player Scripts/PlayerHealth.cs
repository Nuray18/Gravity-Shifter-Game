using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f; // Maksimum sağlık
    private float currentHealth;

    private PlayerHealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth; // Oyuncunun sağlığını maksimum yap

        // PersistentScene yüklendiğinde UI bağlanmış olacak
        StartCoroutine(AssignUI());
    }

    private IEnumerator AssignUI()
    {
        // GameManager içindeki UI'yi bulana kadar bekle
        while (GameManager.instance?.playerHealthUI == null)
        {
            yield return null;
        }

        healthUI = GameManager.instance.playerHealthUI;

        if (healthUI != null)
        {
            healthUI.UpdateHealthText(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogError("PlayerHealthUI GameManager içinde atanmadı!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Hasarı sağlıktan çıkar
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Sağlık 0 ile max arasında kalmalı

        if(healthUI != null)
        {
            healthUI.UpdateHealthText(currentHealth, maxHealth); // Sağlık UI'yi güncelle
        }

        if (currentHealth <= 0)
        {
            Die(); // Sağlık 0 ise ölme işlemi
        }
    }
    
    public void ResetHealth() // Yeni fonksiyon!
    {
        currentHealth = maxHealth;

        if (healthUI != null)
        {
            healthUI.UpdateHealthText(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");

        GameOverPanel gameOverPanel = GameManager.instance?.GetGameOverPanel();

        if (gameOverPanel != null)
        {
            gameOverPanel.ShowGameOver();
        }
        else
        {
            Debug.LogError("GameOverPanel bulunamadı!");
        }

        Time.timeScale = 0f;
    }
}
