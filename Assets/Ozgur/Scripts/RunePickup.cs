using UnityEngine;

public class RunePickup : MonoBehaviour
{
    [SerializeField] private bool isCollected;
    [SerializeField] private ParticleSystem runeParticle;
    [SerializeField] private Material newSkybox;
    private AudioSource aus;
    private MeshRenderer mr;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
        mr = GetComponent<MeshRenderer>();
    }
    
    private void ChangeSkybox()
    {
        RenderSettings.skybox = newSkybox; // Assign the new skybox material
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        if (isCollected) return;

        isCollected = true;
        aus.Play();
        mr.enabled = false;
        runeParticle.Play();
        ChangeSkybox();
        
        if (CompareTag("RedRune")) PlayerColorEnabler.EnableRedColor();
        if (CompareTag("GreenRune")) PlayerColorEnabler.EnableGreenColor();
        if (CompareTag("BlueRune")) PlayerColorEnabler.EnableBlueColor();
        if (CompareTag("YellowRune")) PlayerColorEnabler.EnableYellowColor();
    }
}
