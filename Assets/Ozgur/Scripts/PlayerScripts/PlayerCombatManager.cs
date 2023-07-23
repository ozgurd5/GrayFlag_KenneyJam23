using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float animationDuration = 0.1f;
    [SerializeField] private AudioSource aus;
    
    private PlayerInputManager pim;
    private GameObject sword;

    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
        sword = GameObject.Find("PlayerCamera/Sword");
    }

    private void Update()
    {
        if (!pim.isAttackKeyDown) return;
        StartCoroutine(PlaySwordAnimation());
        aus.Play();
        
        if (!CrosshairManager.isLookingAtEnemy) return;
        CrosshairManager.crosshairHit.collider.GetComponent<EnemyManager>().GetHit(transform.forward);
    }

    private IEnumerator PlaySwordAnimation()
    {
        sword.transform.DOLocalRotate(new Vector3(25f, 0f, 0f), animationDuration);
        yield return new WaitForSeconds(animationDuration);
        sword.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), animationDuration);
    }
}
