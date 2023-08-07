using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float chestOpeningAnimationTime = 0.5f;
    [SerializeField] private float coinAnimationTime = 0.5f;
    [SerializeField] private float coinFlyTime = 0.1f;

    [Header("Assign - Sound")]
    [SerializeField] private AudioClip chestSound;
    [SerializeField] private AudioClip coinSound;
    
    [Header("Info - No Touch")]
    public bool isChestOpened;

    private Transform chestLidTransform;
    private Transform coinTransform;
    private AudioSource aus;
    private Transform playerTransform;
    
    private void Awake()
    {
        chestLidTransform = transform.GetChild(0);
        coinTransform = transform.GetChild(1);
        aus = GetComponent<AudioSource>();
        playerTransform = GameObject.Find("Player").transform;
    }

    public void OpenChest()
    {
        isChestOpened = true;
        
        aus.PlayOneShot(chestSound);
        StartCoroutine(PlayChestOpenAnimation());
    }

    private IEnumerator PlayChestOpenAnimation()
    {
        chestLidTransform.DOLocalRotate(new Vector3(90f, 0f, 0f), chestOpeningAnimationTime);
        yield return new WaitForSeconds(chestOpeningAnimationTime);
        
        aus.PlayOneShot(coinSound);
        coinTransform.DOLocalMoveY(10, coinAnimationTime);
        yield return new WaitForSeconds(coinAnimationTime);
        
        coinTransform.DOMove(playerTransform.position + new Vector3(0f, 0.5f, 0f), coinFlyTime);
        yield return new WaitForSeconds(coinFlyTime);
        
        CoinManager.Singleton.IncreaseCoinNumber();
        Destroy(coinTransform.gameObject);
    }
}