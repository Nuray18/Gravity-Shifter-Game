using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float throwForce = 20f; // Fırlatma gücü
    public float explosionDelay = 3f; // Patlama süresi
    public float explosionRadius = 5f; // Patlama yarıçapı
    public float explosionForce = 700f; // Patlama gücü
    public int damage = 50; // Verilecek hasar
    public GameObject explosionEffect; // Patlama efekti (opsiyonel)

    private bool hasExploded = false; // Tekrar patlamayı önlemek için
    private Rigidbody rb;

    void Start()
    {
        // Patlamayı gecikmeli başlat
        Invoke("Explode", explosionDelay);
    }

    void Explode()
    {
        if (hasExploded) return; // Eğer zaten patladıysa çık
        hasExploded = true;

        // Patlama efekti oluştur
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Patlama alanındaki objeleri bul
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // **Rigidbody varsa itme kuvveti uygula**
            Rigidbody targetRb = nearbyObject.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                targetRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // **Düşman veya Player’a hasar ver**
            if (nearbyObject.CompareTag("Enemy"))
            {
                // Bu kod ile her bir dusman icin ayri if state yazmadan tum dusmanlar icin ortak health yaptim.
                EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
            else if (nearbyObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }

        // **Bomba nesnesini yok et**
        Destroy(gameObject);
    }
}
