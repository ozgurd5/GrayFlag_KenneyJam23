using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private int health = 10;
    [SerializeField] private int knockbackForce = 1000;
    [SerializeField] private float damageTakingAnimTime = 0.5f;
    
    [Header("Colliders")]
    [SerializeField] private Collider aliveCollider;
    [SerializeField] private Collider deadCollider;

    private Rigidbody rb;
    private Animator an;

    private RaycastHit hit;
    
    public enum EnemyState
    {
        Walking,
        Punching,
        Dead
    }

    public EnemyState currentState;
    public bool isDamageTaking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
    }

    public void EnterPunchingState()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Punching;
        an.Play("Zombie Punching");
    }

    public void EnterWalkingState()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Walking;
        an.Play("ZombieWalking");
    }

    public void GetHit(Vector3 playerTransformForward)
    {
        if (currentState == EnemyState.Dead) return;
        
        an.applyRootMotion = false;
        isDamageTaking = true;
        
        health -= 3;
        if (CheckForDeath()) return;
        
        an.Play("Zombie Reaction Hit More");
        rb.AddForce(knockbackForce * playerTransformForward, ForceMode.Acceleration);
        Invoke(nameof(ResetAfterDamage), damageTakingAnimTime);
    }
    
    private void ResetAfterDamage()
    {
        an.applyRootMotion = true;
        isDamageTaking = false;
    }
    
    private bool CheckForDeath()
    {
        if (health < 0)
        {
            currentState = EnemyState.Dead;
            an.Play("Zombie Death");
            
            aliveCollider.enabled = false;
            deadCollider.enabled = true;
            return true;
        }

        else return false;
    }
}