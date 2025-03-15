using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChasingTarget,
        Attacking,
        GoingBackToStart,
    }

    [SerializeField] private float targetRange = 50f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float stopChaseDistance = 80f;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float findTargetInterval = 1f;

    private EnemyMovement enemyMovement;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private State currentState;
    [SerializeField]
    private Transform player;
    private float nextAttackTime;
    private float nextFindTargetTime;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        currentState = State.Roaming;
    }

    private void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
    }
    private void Update()
    {
        switch (currentState)
        {
            case State.Roaming:
                HandleRoaming();
                break;
            case State.ChasingTarget:
                HandleChasing();
                break;
            case State.Attacking:
                HandleAttacking();
                break;
            case State.GoingBackToStart:
                HandleGoingBack();
                break;
        }
    }

    private void HandleRoaming()
    {
        enemyMovement.MoveToTarget(roamPosition);
        if (Vector3.Distance(transform.position, roamPosition) < 1f)
        {
            roamPosition = GetRoamingPosition();
        }

        if (Time.time >= nextFindTargetTime)
        {
            FindTarget();
            nextFindTargetTime = Time.time + findTargetInterval;
        }
    }

    private void HandleChasing()
    {
        if (player == null) { currentState = State.Roaming; return; }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRange && Time.time > nextAttackTime)
        {
            enemyMovement.StopMovement();
            currentState = State.Attacking;
            StartCoroutine(AttackPlayer());
            nextAttackTime = Time.time + attackCooldown;
        }
        else if (distanceToPlayer > stopChaseDistance)
        {
            currentState = State.GoingBackToStart;
        }
        else
        {
            enemyMovement.MoveToTarget(player.position);
        }
    }

    private void HandleAttacking()
    {
        if (player == null || Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = State.ChasingTarget;
        }
    }

    private void HandleGoingBack()
    {
        enemyMovement.MoveToTarget(startingPosition);
        if (Vector3.Distance(transform.position, startingPosition) < 1f)
        {
            currentState = State.Roaming;
        }
    }

    private IEnumerator AttackPlayer()
    {
        if (player == null) yield break;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        yield return new WaitForSeconds(1f);

        if (player != null && Vector3.Distance(transform.position, player.position) < attackRange)
        {
            // this is a recursion
            //StartCoroutine(AttackPlayer());
        }
        else
        {
            currentState = State.ChasingTarget;
        }
    }

    private void FindTarget()
    {
        if (Player.Instance == null) return;

        if (Vector3.Distance(transform.position, Player.Instance.GetPosition()) < targetRange)
        {
            currentState = State.ChasingTarget;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + GetRandomDir() * Random.Range(10f, 70f);
    }

    private static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
}
