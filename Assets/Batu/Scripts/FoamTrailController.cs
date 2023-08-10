using UnityEngine;

public class FoamTrailController : MonoBehaviour
{
    public ParticleSystem foamParticles;
    public float maxVelocity = 60f; //ship max speed
    public float maxEmissionRate = 30f; // particle speed according to ship

    private Rigidbody shipRigidbody;

    void Start()
    {
        shipRigidbody = GetComponent<Rigidbody>();
        foamParticles.Stop();
    }

    void Update()
    {
        float forwardVelocity = Vector3.Dot(shipRigidbody.velocity, transform.forward);

        if (forwardVelocity > 0f) // Ship is moving forward
        {
            float normalizedVelocity = Mathf.Clamp01(forwardVelocity / maxVelocity);
            var emission = foamParticles.emission;
            emission.rateOverDistanceMultiplier = normalizedVelocity * maxEmissionRate;
            foamParticles.Play();
        }
        else
        {
            foamParticles.Stop();
        }
    }
}