using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public TextMeshProUGUI healthText; // UI'deki Text
    private Color normalColor;
    private Color lowHealthColor = Color.red; // Düşük sağlık için kırmızı renk
    private bool isLowHealth = false;
    private float pulseSpeed = 0.5f; // Pulse hızını ayarlama
    private float pulseAmount = 1.2f; // Pulse miktarı (büyüme oranı)
    private float pulseTime = 0f;

    // burda level1 de gamemanager ve bu script birbirlerini bulduklarinda 
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.playerHealthUI = this;
        }
    }

    void Start()
    {
        normalColor = healthText.color; // Metnin varsayılan rengini normal renge ata
        healthText.color = normalColor; // Başlangıçta doğru rengi ayarla
    }


    void Update()
    {
        if (isLowHealth)
        {
            PulseAnimation(); // Pulse animasyonu
        }
    }

    public void UpdateHealthText(float currentHealth, float maxHealth)
    {
        healthText.text = currentHealth.ToString("F0");

        if (currentHealth <= 20)
        {
            Color newColor = lowHealthColor;
            newColor.a = 1f; // Alpha değerini tam görünür yap
            healthText.color = newColor;

            if (!isLowHealth)
            {
                isLowHealth = true;
            }
        }
        else
        {
            Color newColor = normalColor;
            newColor.a = 1f; // Alpha değerini tam görünür yap
            healthText.color = newColor;

            isLowHealth = false;
        }
    }

    // Pulse animasyonu fonksiyonu
    void PulseAnimation()
    {
        pulseTime += Time.deltaTime * pulseSpeed; // Zamanla artan bir değer
        float scale = Mathf.PingPong(pulseTime, pulseAmount - 1) + 1; // Yavaşça büyüyüp küçülen değer
        healthText.transform.localScale = new Vector3(scale, scale, 1); // Metni büyütüp küçült
    }
}
