using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private int health = 10;
    [SerializeField] private int knockbackForce = 1000;

    private Rigidbody rb;
    private Animator an;
    private PlayerDetection pd;
    private PlayerDamageManager playerDamageManager;

    private RaycastHit hit;
    
    public enum EnemyState
    {
        Idle,
        Walking,
        Punching,
        Dead
    }

    public EnemyState currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
        pd = GetComponentInChildren<PlayerDetection>();
        playerDamageManager = GameObject.Find("Player").GetComponent<PlayerDamageManager>();

        pd.OnPlayerEnter += EnterPunchingState;
        pd.OnPlayerExit += ExitPunchingState;
    }

    private void EnterPunchingState()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Punching;
        an.Play("Zombie Punching");
        playerDamageManager.GetHit(transform.forward);
    }

    private void ExitPunchingState()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Walking;
        an.Play("ZombieWalking");
    }
    
    public void GetHit(Vector3 playerTransformForward)
    {
        health -= 3;
        an.Play("Zombie Reaction Hit More");
        rb.AddForce(knockbackForce * playerTransformForward, ForceMode.Acceleration);
        CheckForDeath();
    }
    
    private void CheckForDeath()
    {
        if (health < 0)
        {
            currentState = EnemyState.Dead;
            an.Play("Zombie Death");
        }
    }
}