using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isMovingToTarget = false;
    public float patrolRadius = 10f; // Gezinti yarıçapı
    public Transform target; // Eğer hedef belirlenirse takip edecek

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(Patrol());
    }

    private void Update()
    {
        if (target != null)
        {
            MoveToTarget(target.position);
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            if (target == null) // Eğer hedef yoksa devriye gezsin
            {
                isMovingToTarget = true;
                Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
                randomDirection += transform.position;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
                {
                    MoveToTarget(hit.position);
                    yield return new WaitUntil(() => HasReachedDestination());
                }

                isMovingToTarget = false;
                yield return new WaitForSeconds(Random.Range(2f, 4f)); // 2-4 saniye bekle
            }
        }
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            isMovingToTarget = true;
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
    }

    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            isMovingToTarget = false;
        }
    }

    private bool HasReachedDestination()
    {
        if (agent == null || !agent.isOnNavMesh) return false;
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
