﻿using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Assign - Values")]
    [SerializeField] private float walkPointRange = 10f;
    [SerializeField] private float sightRange = 20f;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float walkingSpeed; //zombie: 0.1 - skeleton: 5
    [SerializeField] private float runningSpeed; //zombie: 5 - skeleton: 15
    
    [Header("No Touch - Info")]
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool isWalkPointSet;
    [SerializeField] private bool isPlayerInSightRange;
    [SerializeField] private bool isPlayerInAttackRange;
    [SerializeField] private bool didEncounterPlayer;
    [SerializeField] private Collider[] overlapSphereColliders;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private EnemyManager em;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        em = GetComponent<EnemyManager>();

        PlayerDamageManager.OnPlayerDeath += ResetAfterPlayerDeath;
    }

    private void Start()
    {
        em.EnterWalkingState();
    }

    private void Update()
    {
        if (em.currentState == EnemyManager.EnemyState.Dead) return;
        if (em.currentState == EnemyManager.EnemyState.GettingDamage)
        {
            navMeshAgent.isStopped = true;
            return;
        }
        
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!isPlayerInSightRange && !isPlayerInAttackRange && !didEncounterPlayer) Patrolling();
        else if (isPlayerInAttackRange && isPlayerInSightRange) AttackPlayer();
        else if ((isPlayerInSightRange && !isPlayerInAttackRange) || didEncounterPlayer) ChasePlayer();
    }

    private void Patrolling()
    {
        if (!isWalkPointSet) SearchWalkPoint();
        else if (isWalkPointSet) navMeshAgent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) isWalkPointSet = false;
        
        if (em.currentState != EnemyManager.EnemyState.Walking) em.EnterWalkingState();

        navMeshAgent.speed = walkingSpeed;
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Walk point must must be in open space, not inside of an object. Check the note in bellow.
        overlapSphereColliders = Physics.OverlapSphere(walkPoint, 2f);
        bool isOverlapping = overlapSphereColliders.Length > 1; //We must ignore the ground which is always overlapping.

        //Walk point must be over the ground, not in air.
        bool isThereGround = Physics.Raycast(walkPoint, -transform.up, 0.01f, groundLayer);

        if (isOverlapping && isThereGround) isWalkPointSet = true;

        //The note in bellow: We must not use OverlapSphereNonAlloc because it doesn't count initial colliders. These are the ones I want to count..
        //..but it doesn't. I think it's work like sphereCast. Unit docs note about sphereCast:
        //SphereCast will not detect colliders for which the sphere overlaps the collider.
    }

    private void AttackPlayer()
    {
        navMeshAgent.isStopped = true;
        
        transform.LookAt(player, Vector3.up);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        if (em.currentState != EnemyManager.EnemyState.Attack) em.EnterAttackState();
    }

    private void ChasePlayer()
    {
        if (em.currentState == EnemyManager.EnemyState.GettingDamage) return;
        
        navMeshAgent.isStopped = false;
        didEncounterPlayer = true;
        
        if (em.currentState == EnemyManager.EnemyState.Attack) em.StopAttack();
        if (em.currentState != EnemyManager.EnemyState.Running) em.EnterRunningState();
        
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = runningSpeed;
    }

    private void ResetAfterPlayerDeath()
    {
        didEncounterPlayer = false;
        navMeshAgent.isStopped = false;
    }

    private void OnDestroy()
    {
        PlayerDamageManager.OnPlayerDeath -= ResetAfterPlayerDeath;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(walkPoint, 2f);
    }
}
