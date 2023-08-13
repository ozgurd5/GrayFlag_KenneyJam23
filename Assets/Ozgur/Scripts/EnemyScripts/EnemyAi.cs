using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Assign - Values")]
    [SerializeField] private float walkPointRange = 5f;
    [SerializeField] private float sightRange = 20f;
    [SerializeField] private float attackRange = 7f;
    
    private Transform player;
    private NavMeshAgent meshAgent;
    private EnemyManager em;
    
    private Vector3 walkPoint;
    private bool isWalkPointSet;
    
    private bool playerInSightRange;
    private bool playerInAttackRange;
    
    private bool didEncounterPlayer;
    private float timeBetweenAttacks;
    private bool isAttacking;

    private IEnumerator attackRoutine;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        meshAgent = GetComponent<NavMeshAgent>();
        em = GetComponent<EnemyManager>();

        attackRoutine = em.EnterAttackState();

        //Disable for play-mode testing
        //timeBetweenAttacks = em.preparingForAttackAnimTime + 0.05f;
    }

    private void Start()
    {
        em.EnterWalkingState();
    }

    private void Update()
    {
        //Enable while play-mode testing
        timeBetweenAttacks = em.attackPrepareTime + 0.05f;
        
        if (em.currentState == EnemyManager.EnemyState.Dead) return;
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange && !didEncounterPlayer) Patrolling();
        else if (playerInAttackRange && playerInSightRange) AttackPlayer();
        else if ((playerInSightRange && !playerInAttackRange) || didEncounterPlayer) ChasePlayer();
    }

    private void Patrolling()
    {
        if (!isWalkPointSet) SearchWalkPoint();
        else if (isWalkPointSet) meshAgent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) isWalkPointSet = false;
        
        if (em.currentState != EnemyManager.EnemyState.Walking) em.EnterWalkingState();
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) isWalkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (em.isTakingDamage) return;
        
        didEncounterPlayer = true;
        StopCoroutine(attackRoutine);
        if (em.currentState != EnemyManager.EnemyState.Running) em.EnterRunningState();
        meshAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        meshAgent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!isAttacking)
        {
            isAttacking = true;
            
            attackRoutine = em.EnterAttackState();
            StartCoroutine(attackRoutine);
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
