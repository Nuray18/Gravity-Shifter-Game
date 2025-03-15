using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explotionEffect;

    public float delay = 5f;

    private float damage = 50f;
    private float explosionRadius = 7f;

    private float countDown;
    private bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countDown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if(countDown <= 0f && hasExploded == false)
        {
            Explode();
            hasExploded = true;
        }
    }

    private void Explode()
    {
        // Show effect
        Instantiate(explotionEffect, transform.position, transform.rotation);

        // get nearby objects

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Sadece "Enemy" tag'ine sahip objelere etki et
            if (nearbyObject.CompareTag("EnemyBody"))
            {
                // Düşmanın health componentini bul
                IHealth enemyHealth = nearbyObject.GetComponent<IHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }

        // remove grenade (destroy GameObject)
        Destroy(gameObject);
    }
}
