using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float punchAnimTime = 0.2f;
    
    [Header("Assign - Values")]
    [SerializeField] private float walkPointRange = 5f;
    [SerializeField] private float sightRange = 20f;
    [SerializeField] private float attackRange = 5f;
    
    private Transform player;
    private NavMeshAgent meshAgent;
    private EnemyManager em;
    
    private Vector3 walkPoint;
    private bool walkPointSet;

    private float timeBetweenAttacks;
    private bool isAttacking;
    private bool playerInSightRange;
    private bool playerInAttackRange;
    
    private bool walkingFromPunchingFlag;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        meshAgent = GetComponent<NavMeshAgent>();
        
        em = GetComponent<EnemyManager>();

        timeBetweenAttacks = em.preparingForAttackAnimTime + 0.05f;
    }

    private void Start()
    {
        em.EnterWalkingState();
    }

    private void Update()
    {
        if (em.currentState == EnemyManager.EnemyState.Dead) return;
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        else if (walkPointSet) meshAgent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        
        if (em.currentState != EnemyManager.EnemyState.Walking) em.EnterWalkingState();
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (em.isDamageTaking) return;

        if (em.currentState == EnemyManager.EnemyState.Attack && !walkingFromPunchingFlag) StartCoroutine(EnterWalkingFromPunching());
        else if (em.currentState == EnemyManager.EnemyState.Walking) em.EnterRunningState();

        meshAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        meshAgent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!isAttacking)
        {
            isAttacking = true;
            
            StartCoroutine(em.EnterAttackState());
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        isAttacking = false;
    }

    private IEnumerator EnterWalkingFromPunching()
    {
        walkingFromPunchingFlag = true;
        yield return new WaitForSeconds(punchAnimTime);
        
        em.EnterWalkingState();
        walkingFromPunchingFlag = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
