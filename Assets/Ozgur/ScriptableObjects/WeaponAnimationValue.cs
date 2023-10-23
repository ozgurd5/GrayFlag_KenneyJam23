using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WeaponAnimationValue", fileName = "WeaponAnimationValue")]
public class WeaponAnimationValue : ScriptableObject
{
    [Header("Assign - Durations")]
    public float attackAnimationHalfDuration = 0.1f;
    public float walkingAnimationHalfDuration = 0.5f;
    public float runningAnimationHalfDuration = 0.2f;
    public float runningModeSwitchDuration = 0.2f;

    [Header("Assign - Rotations")]
    public float defaultRotationY = 0f;
    public float attackRotationX = 50f;
    public float runningModeRotationX = 25f;
    public float walkingModeRotationX = 0f;

    [Header("Assign - Positions")]
    public float movingAnimationPositionY = -0.35f;
    public float movingAnimationPositionYBack = -0.4f;
    public float walkingModeMovingAnimationPositionZ = 0.62f;
    public float walkingModeMovingAnimationPositionZBack = 0.6f;
    public float runningModeMovingAnimationPositionZ = 0.52f;
    public float runningModeMovingAnimationPositionZBack = 0.5f;

    [Header("Assign - Hiding")]
    public float hidingTime = 0.3f;
    public float hiddenPositionY = -1.1f;
    public float hiddenPositionYBack = -0.4f;
}
