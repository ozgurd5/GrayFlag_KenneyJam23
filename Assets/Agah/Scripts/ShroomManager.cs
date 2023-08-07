using UnityEngine;
using TMPro;


public class ShroomManager : MonoBehaviour 
{
    [SerializeField]TMP_Text shroomText;
    int mushCollected = 0;
    int mushAmount;

    private void Awake()
    {
        ShroomPickup.onPickup += OnShroomPickup;
        mushAmount = ShroomPickup.count;
    }
    private void OnShroomPickup()
    {
        mushCollected++;
        ShowShroomText();
    }
    void ShowShroomText()
    {
        shroomText.text = mushCollected.ToString() + "/" + mushAmount.ToString();
    }
}
