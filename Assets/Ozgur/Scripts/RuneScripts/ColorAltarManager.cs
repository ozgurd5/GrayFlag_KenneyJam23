using UnityEngine;

public class ColorAltarManager : MonoBehaviour
{
    public static int activatedAltars;
    
    [Header("Assign")]
    [SerializeField] private Transform targetPointTransform;
    
    [Header("Info - No Touch")]
    public bool isActivated;

    private GameObject rune;
    private LineRenderer lr;
    private AudioSource aus;
    private Transform laserPointTransform;

    private void Awake()
    {
        rune = transform.GetChild(0).gameObject;
        lr = GetComponent<LineRenderer>();
        aus = GetComponent<AudioSource>();
        laserPointTransform = transform.GetChild(2);
    }

    public void EnableAltar()
    {
        isActivated = true;
        
        rune.SetActive(true);
        aus.Play();
        
        lr.enabled = true;
        lr.SetPosition(0, laserPointTransform.position);
        lr.SetPosition(1, targetPointTransform.position);

        activatedAltars++;
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (activatedAltars == 4) Debug.Log("Completed");
    }
}
