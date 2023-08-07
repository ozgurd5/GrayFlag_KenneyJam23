using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Assign")][SerializeField] private TextMeshProUGUI coinText;
                      [SerializeField]private TextMeshProUGUI chestText;
    [Header("Info - No Touch")] [SerializeField] private int coinNumber; private int chestCollected = 0, chestAmount;
    

    public static CoinManager Singleton;

    private void Awake()
    {
        Singleton = GetComponent<CoinManager>();
        ChestManager.OnChestPickup += ChestPickup_OnChestPickup;
        chestAmount = ChestManager.count;
    }
    

    private void ChestPickup_OnChestPickup()
    {
        chestCollected++;
        chestText.text = chestCollected.ToString() + "/" + chestAmount.ToString();  // Not: Edit�rde x2 g�sterecek chestAmount'u. Buildde d�zg�n �al���yor.
    }

    public void IncreaseCoinNumber()
    {
        coinNumber += 3;
        coinText.text = $"Coins: {coinNumber}";
    }


}