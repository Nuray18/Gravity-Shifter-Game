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
        //if (agent != null)
        //{
        //    Debug.Log(gameObject.name + " isOnNavMesh: " + agent.isOnNavMesh);
        //}

        if (isPlayerInZone && player != null && agent != null && agent.isOnNavMesh)
        {
            // Oyuncuyu takip et
            agent.SetDestination(player.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;

            // Oyuncunun sadece yerçekimi değiştirme özelliğini kapat
            if (playerScript != null)
            {
                playerScript.canChangeGravity = false; // Yerçekimi değiştirme devre dışı
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;

            // Oyuncunun yerçekimi değiştirme özelliğini yeniden aktif et
            if (playerScript != null)
            {
                playerScript.canChangeGravity = true; // Yerçekimi değiştirme yeniden etkin
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
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (agent != null)
        {
            agent.Warp(initialPosition); // Karakteri sıfırla
            agent.isStopped = false;
            agent.ResetPath();
        }

        ResetAI();
        GetComponent<GravityEaterHealth>()?.ResetHealth();

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

        // enemyleri EnemyManager'deki enemies arrayinden siliyoruz
        EnemyManager.instance.UnregisterEnemy(this);

        // Oyun nesnesini yok et
        Destroy(gameObject);
    }
}
