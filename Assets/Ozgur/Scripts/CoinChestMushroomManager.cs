using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class CoinChestMushroomManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI chestText;
    [SerializeField] private TextMeshProUGUI mushroomText;
    
    [Header("Info - No Touch")]
    public int coinNumber;
    [SerializeField] private int chestNumber;
    [SerializeField] private int mushroomNumber;

    static int _chestNumber, _mushroomNumber, _coinNumber;

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
        coinText.text = $"{coinNumber}";
        _coinNumber = coinNumber;
    }
    
    public void IncreaseChestNumber()
    {
        chestNumber++;
        chestText.text = chestNumber + "/" + ChestManager.totalChestCount;
        _chestNumber = chestNumber;    
    }

    public void IncreaseMushroomNumber()
    {
        mushroomNumber++;
        mushroomText.text = mushroomNumber + "/" + MushroomManager.totalMushroomNumber;
        mushroomSource.Play();
        _mushroomNumber = mushroomNumber;
    }

    public static int GetNumber(string objNumberToGet)
    {
        if (objNumberToGet == "Chest")
            return _chestNumber;
        else if (objNumberToGet == "Mushroom")
            return _mushroomNumber;
        else if (objNumberToGet == "Coin")
            return _coinNumber;
        else return 0;
    }

}