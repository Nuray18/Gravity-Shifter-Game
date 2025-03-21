using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyGravityEater : MonoBehaviour, IEnemy
{
    public float patrolRadius = 10f; // Gezinti alanı yarıçapı
    private NavMeshAgent agent; // Düşman hareketini kontrol eder

    public Transform player; // Oyuncunun Transform'u player objecti bunu olusturmamizin nedeni player objectinin icinde yani inspectorda ona bagli olan gerekli scriptleri direk cagirmaktir.
                             // 
    public float detectionRadius = 5f; // Alan yarıçapı
    public LayerMask playerLayer; // Oyuncuyu algılamak için Layer

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isPlayerInZone = false; // Oyuncu alanda mı?
    private bool isMovingToTarget = false; // Yeni hedefe gidiyor mu?

    private Player playerScript; // Oyuncunun yerçekimi kontrolü için
    protected PlayerHealth playerHealthScript; // Oyuncunun enemy ile olan iletisiminden aldigi hasari hesaplamak icin.

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Start()
    {
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
            playerHealthScript = player.GetComponent<PlayerHealth>(); // 
        }

        EnemyManager.instance.RegisterEnemy(this);
        StartCoroutine(Patrol()); // Gezinti davranışı başlasın
    }

    void Update()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            if (isPlayerInZone && player != null)
            {
                if (!agent.pathPending && agent.isStopped == false)
                {
                    agent.SetDestination(player.position);
                }
            }
            else if (!isMovingToTarget) // Oyuncu alanda değilse tekrar gezmeye başla
            {
                StartCoroutine(Patrol());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            agent.isStopped = false;  // Hareketi aktif et

            if (playerScript != null)
            {
                playerScript.canChangeGravity = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player çıkış yaptı, gravity değiştirilebilir!");
            isPlayerInZone = false;
            agent.isStopped = false;

            if (playerScript != null)
            {
                playerScript.canChangeGravity = true;
            }
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            // Eğer oyuncu algılanmıyorsa ve yeni bir hedefe gitmiyorsa yeni hedef belirle
            if (!isPlayerInZone && !isMovingToTarget)
            {
                isMovingToTarget = true; // Yeni hedefe gidiyor
                Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
                randomDirection += initialPosition;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
                {
                    agent.SetDestination(hit.position);

                    // Hedefe ulaşana kadar bekle
                    yield return new WaitUntil(() => HasReachedDestination());

                    // Yeni hedef belirlenebilir
                    isMovingToTarget = false;
                }
            }

            // Duraklama süresi
            yield return new WaitForSeconds(2f);
        }
    }

    private bool HasReachedDestination()
    {
        if (agent == null || !agent.isOnNavMesh) return false;

        // Hedefe ulaşıp ulaşmadığını kontrol et
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
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

    // bu bir interface funcudur ve her bir classta yani farkli enemylerde farkli sekilde kullanilabilir
    public void ResetEnemy()
    {
        StopAllCoroutines();
        isMovingToTarget = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

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
