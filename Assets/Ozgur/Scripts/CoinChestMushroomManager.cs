using TMPro;
using UnityEngine;

public class CoinChestMushroomManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI chestText;
    [SerializeField] private TextMeshProUGUI mushroomText;
    
    [Header("Info - No Touch")]
    [SerializeField] private int coinNumber;
    [SerializeField] private int chestNumber;
    [SerializeField] private int mushroomNumber;

    //The reason why every mushroom doesn't have it's own audio source is they destroy before the audio clip ends
    private AudioSource mushroomSource;
    
    public static CoinChestMushroomManager Singleton;
    
    private void Awake()
    {
        Singleton = GetComponent<CoinChestMushroomManager>();
        mushroomSource = GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        chestText.text = chestNumber + "/" + ChestManager.totalChestCount;
        mushroomText.text = mushroomNumber + "/" + MushroomManager.totalMushroomNumber;
    }
    
    public void IncreaseCoinNumber()
    {
        coinNumber += 3;
        coinText.text = $"Coins: {coinNumber}";
    }
    
    public void IncreaseChestNumber()
    {
        chestNumber++;
        chestText.text = chestNumber + "/" + ChestManager.totalChestCount;
    }

    public void IncreaseMushroomNumber()
    {
        mushroomNumber++;
        mushroomText.text = mushroomNumber + "/" + MushroomManager.totalMushroomNumber;
        mushroomSource.Play();
    }
}