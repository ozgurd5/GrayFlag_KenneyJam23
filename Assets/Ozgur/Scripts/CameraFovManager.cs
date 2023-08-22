using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraFovManager : MonoBehaviour
{
    [Header("Assign")] [SerializeField] private float increaseAmount = 10f;
    [SerializeField] private float fovChangeTime = 0.1f;
    
    [Header("Info - No Touch")]
    [SerializeField] private float defaultFov;
    [SerializeField] private bool isFovIncreased;

    private CinemachineVirtualCamera cam;
    private PlayerStateData psd;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        psd = PlayerStateData.Singleton;
        
        defaultFov = cam.m_Lens.FieldOfView;
    }

    private void Update()
    {
        if (psd.isRunning && !isFovIncreased)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFov(true));
        }
        
        else if (!psd.isRunning && isFovIncreased)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFov(false));
        }
    }

    private IEnumerator ChangeFov(bool isIncreasing)
    {
        float timePassed = 0f;
        float speed = increaseAmount / fovChangeTime;

        while (timePassed <= fovChangeTime)
        {
            if (isIncreasing) cam.m_Lens.FieldOfView += speed * Time.deltaTime;
            else cam.m_Lens.FieldOfView -= speed * Time.deltaTime;
            
            if (isIncreasing && (cam.m_Lens.FieldOfView >= defaultFov + increaseAmount)) break;
            else if (!isIncreasing && (cam.m_Lens.FieldOfView <= defaultFov)) break;

            timePassed += Time.deltaTime;
            yield return null;
        }

        if (isIncreasing) cam.m_Lens.FieldOfView = defaultFov + increaseAmount;
        else cam.m_Lens.FieldOfView = defaultFov;

        isFovIncreased = isIncreasing;
    }
}
