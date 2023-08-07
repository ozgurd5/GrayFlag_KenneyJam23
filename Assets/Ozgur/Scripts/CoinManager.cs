using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Assign")] [SerializeField] private TextMeshProUGUI coinText;
    [Header("Info - No Touch")] [SerializeField] private int coinNumber;

    public static CoinManager Singleton;

    private void Awake()
    {
        Singleton = GetComponent<CoinManager>();
    }

    public void IncreaseCoinNumber()
    {
        coinNumber += 3;
        coinText.text = $"Coins: {coinNumber}";
    }
}