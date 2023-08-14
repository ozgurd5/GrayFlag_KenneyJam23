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
    private NavMeshAgent navMeshAgent;
    private EnemyManager em;
    
    private Vector3 walkPoint;
    private bool isWalkPointSet;
    
    private bool isPlayerInSightRange;
    private bool isPlayerInAttackRange;
    
    private bool didEncounterPlayer;
    private bool isAttacking;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        em = GetComponent<EnemyManager>();
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
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) isWalkPointSet = true;
    }

    private void AttackPlayer()
    {
        navMeshAgent.isStopped = true;
        transform.LookAt(player, Vector3.up);

        if (em.currentState != EnemyManager.EnemyState.Attack) em.AttackPlayer();
    }

    private void ChasePlayer()
    {
        if (em.currentState == EnemyManager.EnemyState.GettingDamage) return;
        
        navMeshAgent.isStopped = false;
        didEncounterPlayer = true;

        em.StopAttack();
        if (em.currentState != EnemyManager.EnemyState.Running) em.EnterRunningState();
        navMeshAgent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
