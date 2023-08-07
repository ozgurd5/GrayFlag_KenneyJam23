using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MushroomManager : MonoBehaviour
{
    public static int totalMushroomNumber;

    [Header("Assign")]
    [SerializeField] private float mushroomFlyTime = 0.1f;

    [Header("Info - No Touch")] public bool isCollected;    //just in case
    
    private Transform playerTransform;

    private void Awake()
    {
        totalMushroomNumber++;
        playerTransform = GameObject.Find("Player").transform;
    }

    public void CollectMushroom()
    {
        CoinChestMushroomManager.Singleton.IncreaseMushroomNumber();
        isCollected = true;
        StartCoroutine(PlayMushroomCollectAnimation());
    }

    private IEnumerator PlayMushroomCollectAnimation()
    {
        transform.DOMove(playerTransform.position + new Vector3(0f, 0.5f, 0f), mushroomFlyTime);
        yield return new WaitForSeconds(mushroomFlyTime + 0.1f);

        Destroy(gameObject);
    }
}
