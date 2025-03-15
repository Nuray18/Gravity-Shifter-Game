using UnityEngine;

public class SawRotator : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float damage = 10f; // Testerenin verdiği hasar

    public float damageInterval = 0.5f; // Hasar verme aralığı (saniye)

    private float damageTimer = 0f; // Zamanlayıcı

    private void Update()
    {
        // Testerenin dönmesini sağla
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Zamanlayıcıyı her çerçevede artır
        damageTimer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Eğer çarpan obje "Player" ise
        {
            if (damageTimer >= damageInterval) // Hasar verme süresi dolmuşsa
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage); // Oyuncunun sağlığını azalt
                }

                damageTimer = 0f; // Zamanlayıcıyı sıfırla
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Oyuncu testereyle temastan çıktığında
        {
            damageTimer = 0f; // Zamanlayıcıyı sıfırla
        }
    }
}
