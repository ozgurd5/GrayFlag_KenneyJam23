using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class ColorAltarManager : MonoBehaviour
{
    public static event Action OnGameCompleted;
    private static int activatedAltars;
    
    [Header("Assign")]
    [SerializeField] private Transform targetPointTransform;
    [SerializeField] private ParticleSystem energyBall;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float explosionTime = 1f;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    public bool impulse;
    
    [Header("Assign - Sounds")]
    [SerializeField] private AudioSource runePlaceSource;
    [SerializeField] private AudioSource laserSource;
    [SerializeField] private AudioSource explosionSource;

    [Header("Info - No Touch")]
    public bool isActivated;
    private bool isEnergyBallPlaying;

    private GameObject rune;
    private LineRenderer lr;
    private Transform laserPointTransform;
    
    
    
    private void Awake()
    {
        rune = transform.GetChild(0).gameObject;
        lr = GetComponent<LineRenderer>();
        runePlaceSource = GetComponent<AudioSource>();
        laserPointTransform = transform.GetChild(2);
        
    }

    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) OnGameCompleted?.Invoke();
        
    }
    #endif

    public void EnableAltar()
    {
        isActivated = true;
        
        rune.SetActive(true);
        
        runePlaceSource.Play();
        laserSource.Play();
        
        lr.enabled = true;
        lr.SetPosition(0, laserPointTransform.position);
        lr.SetPosition(1, targetPointTransform.position);

        activatedAltars++;
        StartCoroutine(CheckCompletion());

        if (!isEnergyBallPlaying)
        {
            energyBall.Play();
            isEnergyBallPlaying = true;
        }
        
        
    }

    private IEnumerator CheckCompletion()
    {
        if (activatedAltars != 4) yield break;

        foreach (var item in ParticleClose.energyParticles)
        {
            item.PlayAnim();
        }
        
        yield return new WaitForSeconds(2.0f);
        
        explosion.Play(); 
        explosionSource.Play();
        
        MeshDestroy meshDestroyScript = FindObjectOfType<MeshDestroy>();
        if (meshDestroyScript != null)
        {
            meshDestroyScript.DestroyMesh();
            Debug.Log("kırıldı");
        }

        impulseSource.GenerateImpulse();
        
        yield return new WaitForSeconds(explosionTime);
        
        Debug.Log("Game Completed");
        OnGameCompleted?.Invoke();
    }
}
