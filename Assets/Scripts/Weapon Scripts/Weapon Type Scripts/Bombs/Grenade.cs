using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float timer = 2f;
    [SerializeField] private float radius = 3f;
    [SerializeField] private float force = 500f;
    [SerializeField] private float damage = 50f;
    [SerializeField] private GameObject explodeEffect;

    private float countDown;
    private bool hasExploded = false;
    private bool isThrown = false;

    private Rigidbody rb;

    private void Start()
    {
        countDown = timer;
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        rb.drag = 0.5f;
        rb.angularDrag = 0.8f;
    }

    private void Update()
    {
        if (isThrown)
        {
            countDown -= Time.deltaTime;

            if (countDown <= 0 && !hasExploded)
            {
                Explode();
            }
        }
    }

    public void Throw(Vector3 throwDirection, float throwForce)
    {
        rb.isKinematic = false; // **Fırlatınca fiziği aç**
        rb.useGravity = true;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // 🎯 Havada sürtünme ekleyelim ki sonsuza kadar gitmesin
        rb.drag = 1.5f;        // **Havada daha fazla yavaşlama**
        rb.angularDrag = 2f;   // **Dönüş daha yavaş olsun**

        isThrown = true;
    }

    private void Explode()
    {
        hasExploded = true;

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
        Destroy(gameObject);
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;

        float impactSpeed = collision.relativeVelocity.magnitude;
        float slowDownFactor = Mathf.Lerp(0.7f, 0.95f, impactSpeed / 15f);

        rb.velocity *= slowDownFactor;
        rb.angularVelocity *= slowDownFactor;

        if (rb.velocity.magnitude < 0.05f)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isThrown) return;

        rb.velocity *= 0.98f;
        rb.angularVelocity *= 0.95f;
    }
}