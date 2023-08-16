using UnityEngine;

public class ColorAltarManager : MonoBehaviour
{
    public static int activatedAltars;
    
    [Header("Assign")]
    [SerializeField] private Transform targetTransform;
    
    [Header("Info - No Touch")]
    public bool isActivated;

    private GameObject rune;
    private LineRenderer lr;
    private AudioSource aus;

    private void Awake()
    {
        rune = transform.GetChild(0).gameObject;
        aus = GetComponent<AudioSource>();
        
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
    }

    public void EnableAltar()
    {
        isActivated = true;
        
        rune.SetActive(true);
        //aus.Play();
        
        lr.enabled = true;
        lr.SetPosition(1, targetTransform.position);

        activatedAltars++;
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (activatedAltars == 4) Debug.Log("Completed");
    }
}
