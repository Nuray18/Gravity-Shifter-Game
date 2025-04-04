using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float timer = 2;
    private float countDown;
    private float radius = 3;
    private bool hasExploded;
    private float force = 500;
    private float damage = 50f;

    public GameObject explodeEffect;

    private void Start()
    {
        countDown = timer;
    }

    private void Update()
    {
        countDown -= Time.deltaTime;
        if(countDown <= 0 && !hasExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject spawnedParticle = Instantiate(explodeEffect, transform.position, transform.rotation);
        Destroy(spawnedParticle, 1);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
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

        hasExploded = true;
        Destroy(gameObject);
    }

}
