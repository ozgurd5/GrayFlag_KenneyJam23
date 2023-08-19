using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static int totalChestCount;
    
    [Header("Assign")]
    [SerializeField] private float chestOpeningAnimationTime = 0.5f;
    [SerializeField] private float coinAnimationTime = 0.5f;
    [SerializeField] private float coinFlyTime = 0.1f;
    [SerializeField] private ParticleSystem whiteCoinParticle;
    [SerializeField] private ParticleSystem yellowCoinParticle;
    
    private ParticleSystem chestOpenParticle;

    [Header("Assign - Sound")]
    [SerializeField] private AudioClip chestSound;
    [SerializeField] private AudioClip coinSound;
    
    [Header("Info - No Touch")]
    public bool isChestOpened;

    private Transform chestLidTransform;
    private Transform coin1Transform;
    private Transform coin2Transform;
    private Transform coin3Transform;
    private Transform playerTransform;
    private AudioSource aus;

    private void Awake()
    {
        chestLidTransform = transform.GetChild(0);
        coin1Transform = transform.GetChild(1);
        coin2Transform = transform.GetChild(2);
        coin3Transform = transform.GetChild(3);

        playerTransform = GameObject.Find("Player").transform;
        aus = GetComponent<AudioSource>();

        chestOpenParticle = whiteCoinParticle;
        PlayerColorEnabler.OnYellowColorEnabled += EnableYellowParticle;

        totalChestCount++;
    }

    public void OpenChest()
    {
        isChestOpened = true;
        
        aus.PlayOneShot(chestSound);
        CoinChestMushroomManager.Singleton.IncreaseChestNumber();
        chestOpenParticle.Play();
        StartCoroutine(PlayChestOpenAnimation());
    }

    private IEnumerator PlayChestOpenAnimation()
    {
        chestLidTransform.DOLocalRotate(new Vector3(90f, 0f, 0f), chestOpeningAnimationTime);
        yield return new WaitForSeconds(chestOpeningAnimationTime);
        
        aus.PlayOneShot(coinSound);
        coin1Transform.DOLocalMoveY(8, coinAnimationTime);
        coin2Transform.DOLocalMoveY(10, coinAnimationTime);
        coin3Transform.DOLocalMoveY(8, coinAnimationTime);
        yield return new WaitForSeconds(coinAnimationTime);
        
        coin1Transform.DOMove(playerTransform.position + new Vector3(0f, 0.5f, 0f), coinFlyTime);
        coin2Transform.DOMove(playerTransform.position + new Vector3(0f, 0.5f, 0f), coinFlyTime);
        coin3Transform.DOMove(playerTransform.position + new Vector3(0f, 0.5f, 0f), coinFlyTime);
        yield return new WaitForSeconds(coinFlyTime);
        
        CoinChestMushroomManager.Singleton.IncreaseCoinNumber();
        
        Destroy(coin1Transform.gameObject);
        Destroy(coin2Transform.gameObject);
        Destroy(coin3Transform.gameObject);
    }

    private void EnableYellowParticle()
    {
        chestOpenParticle = yellowCoinParticle;
    }

    private void OnDestroy()
    {
        PlayerColorEnabler.OnYellowColorEnabled -= EnableYellowParticle;
    }
}
