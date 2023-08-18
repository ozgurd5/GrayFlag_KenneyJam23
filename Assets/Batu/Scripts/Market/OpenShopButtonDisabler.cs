using UnityEngine;

public class OpenShopButtonDisabler : MonoBehaviour
{
    [SerializeField] private GameObject openShopButton;
    [SerializeField] private GameObject shopCanvas;

    private void Start()
    {
        openShopButton.SetActive(true);
    }
    
    public void DisableButton()
    {
        openShopButton.SetActive(false);
    }
    
    public void EnableButton()
    {
        openShopButton.SetActive(true);
    }
}
