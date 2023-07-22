using UnityEngine;

public class ShipAnimationManager : MonoBehaviour
{
    private Transform frontSail;
    private Transform midSail;
    private Transform backSail;

    private Transform frontSailPivot;
    private Transform midSailPivot;
    private Transform backSailPivot;
    
    private Animator frontSailAnimator;
    private Animator midSailAnimator;
    private Animator backSailAnimator;

    private ShipInputManager sim;

    private void Awake()
    {
        frontSail = transform.Find("FrontSail");
        midSail = transform.Find("MidSail");
        backSail = transform.Find("BackSail");

        frontSailPivot = frontSail.Find("FrontSailPivot");
        midSailPivot = midSail.Find("MidSailPivot");
        backSailPivot = backSail.Find("BackSailPivot");
        
        frontSailAnimator = frontSail.Find("FrontSail").GetComponent<Animator>();
        midSailAnimator = midSail.Find("MidSail").GetComponent<Animator>();
        backSailAnimator = backSail.Find("BackSail").GetComponent<Animator>();
        
        sim = GetComponent<ShipInputManager>();
        ShipController.OnSailChanged += PlayAnimation;
    }

    //TODO: lazy solution, find better
    private void Start()
    {
        midSailAnimator.Play("MidSailUp");
        frontSailAnimator.Play("FrontSailUp");
        backSailAnimator.Play("BackSailUp");
    }

    private void Update()
    {
        if (!ShipController.isShipControlled) return;
    
        //frontSail.RotateAround(frontSailPivot.position, Vector3.up, sim.rotateInput);
        //midSail.RotateAround(midSailPivot.position, Vector3.up, sim.rotateInput);
        //backSail.RotateAround(backSailPivot.position, Vector3.up, sim.rotateInput);
    }

    private void PlayAnimation(ShipController.SailMode sailMode)
    {
        if (sailMode == ShipController.SailMode.Stationary) SetStationary();
        if (sailMode == ShipController.SailMode.HalfSail) SetHalfSail();
        if (sailMode == ShipController.SailMode.FullSail) SetFullSail();
    }

    private void SetStationary()
    {
        midSailAnimator.Play("MidSailUp");
    }

    private void SetHalfSail()
    {
        midSailAnimator.Play("MidSailDown");
        frontSailAnimator.Play("FrontSailUp");
        backSailAnimator.Play("BackSailUp");
    }

    private void SetFullSail()
    {
        frontSailAnimator.Play("FrontSailDown");
        backSailAnimator.Play("BackSailDown");
    }

}
