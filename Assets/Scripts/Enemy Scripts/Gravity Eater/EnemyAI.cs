using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// enemy olmuyor 
// ve enemy saldiri suresini duzelttim 

public class EnemyAI : MonoBehaviour, IEnemy
{
    public Transform player; // Oyuncu
    public float chaseRange = 10f; // Takip mesafesi
    public float attackRange = 1.5f; // Saldırı mesafesi
    public float moveSpeed = 2.0f; // Hız
    public float patrolRadius = 5f; // Devriye alanı

    private NavMeshAgent agent;
    [SerializeField]
    private Player playerScript;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isMovingToTarget = false;
    private bool isPlayerInZone = false;
    private float attackCooldown = 0.3f;
    private float lastAttackTime = 0f;
    private Vector3 initialPosition;
    private float damage = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        initialPosition = transform.position;
        StartCoroutine(Patrol()); // Devriye başlat
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange) // Eğer oyuncu yakınsa
        {
            isPlayerInZone = true;
            isChasing = true;
            StopCoroutine(Patrol()); // Devriyeyi durdur
            agent.SetDestination(player.position);
        }
        else
        {
            isPlayerInZone = false;
            isChasing = false;
            if (!isMovingToTarget)
            {
                StartCoroutine(Patrol()); // Tekrar devriyeye başla
            }
        }

        if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            if (!isPlayerInZone && !isMovingToTarget)
            {
                isMovingToTarget = true;
                Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
                randomDirection += initialPosition;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
                {
                    agent.SetDestination(hit.position);

                    // Hedefe ulaşana kadar bekle
                    yield return new WaitUntil(() => HasReachedDestination());

                    // Hızı yumuşak şekilde düşür
                    yield return SmoothStop(1f);

                    // 2-5 saniye bekleme süresi ekle
                    yield return new WaitForSeconds(Random.Range(2f, 5f));

                    // Hızı yumuşak şekilde başlat
                    yield return SmoothStart(1f);

                    isMovingToTarget = false;
                }
            }
            yield return new WaitForSeconds(0.5f); // Fazladan küçük bekleme süresi
        }
    }

    private IEnumerator SmoothStop(float duration)
    {
        float startSpeed = agent.speed;
        float time = 0f;

        while (time < duration)
        {
            agent.speed = Mathf.Lerp(startSpeed, 0f, time / duration); // Hızı kademeli olarak azalt
            time += Time.deltaTime;
            yield return null;
        }
        agent.speed = 0f;
    }

    private IEnumerator SmoothStart(float duration)
    {
        float targetSpeed = moveSpeed;
        float time = 0f;

        while (time < duration)
        {
            agent.speed = Mathf.Lerp(0f, targetSpeed, time / duration); // Hızı kademeli olarak artır
            time += Time.deltaTime;
            yield return null;
        }
        agent.speed = targetSpeed;
    }

    private bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    IEnumerator AttackPlayer()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("Enemy saldırıyor!");

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackCooldown);

        agent.isStopped = false;
        lastAttackTime = Time.time;
        isAttacking = false;
    }

 
    public void ResetEnemy()
    {
        StopAllCoroutines();
        isMovingToTarget = false;

        transform.position = initialPosition;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.Warp(initialPosition); // Karakteri sıfırla
            agent.isStopped = false;
            agent.ResetPath();
        }

        ResetAI();
        GetComponent<GravityEaterHealth>()?.ResetHealth();

        if (playerScript != null)
        {
            playerScript.canChangeGravity = true;
        }

        gameObject.SetActive(true);
        StartCoroutine(Patrol()); // Patrol'u yeniden başlat
    }

    public void ResetAI()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.Warp(initialPosition);  // Düşmanı sıfır noktasına taşı
            agent.isStopped = false;      // Hareketi devam ettir
            agent.ResetPath();            // Yol bilgisini temizle

            StartCoroutine(Patrol()); // Patrol Coroutine'ini yeniden başlat
        }
    }

    public void Die()
    {
        // NavMeshAgent'i devre dışı bırak
        StopAllCoroutines();

        // NavMeshAgent durdur
        if (agent != null)
        {
            agent.isStopped = true;
        }

        // Oyuncunun gravity değiştirme yetkisini geri ver
        if (playerScript != null)
        {
            playerScript.canChangeGravity = true;
        }

        // enemyleri EnemyManager'deki enemies arrayinden siliyoruz
        EnemyManager.instance.UnregisterEnemy(this);

        // Oyun nesnesini yok et
        Destroy(gameObject);
    }
}
