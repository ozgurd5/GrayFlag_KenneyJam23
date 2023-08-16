using UnityEngine;

public class RunePickup : MonoBehaviour
{
    [SerializeField] private bool isCollected;
    [SerializeField] private ParticleSystem runeParticle;
    private AudioSource aus;
    private MeshRenderer mr;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
        mr = GetComponent<MeshRenderer>();
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        if (isCollected) return;

        isCollected = true;
        aus.Play();
        mr.enabled = false;
        runeParticle.Play();
         
        if (CompareTag("RedRune")) PlayerColorEnabler.EnableRedColor();
        if (CompareTag("GreenRune")) PlayerColorEnabler.EnableGreenColor();
        if (CompareTag("BlueRune")) PlayerColorEnabler.EnableBlueColor();
        if (CompareTag("YellowRune")) PlayerColorEnabler.EnableYellowColor();
    }
}
