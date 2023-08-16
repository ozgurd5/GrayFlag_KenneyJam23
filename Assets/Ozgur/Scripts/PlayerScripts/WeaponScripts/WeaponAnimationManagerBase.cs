using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WeaponAnimationManagerBase : MonoBehaviour
{
    protected IEnumerator PlayAttackAnimation(Transform weaponTransform, float attackRotationX, float attackRotationXBack, float attackRotationY, float attackAnimationHalfDuration)
    {
        weaponTransform.transform.DOLocalRotate(new Vector3(attackRotationX, attackRotationY, 0f), attackAnimationHalfDuration);
        yield return new WaitForSeconds(attackAnimationHalfDuration);
        weaponTransform.transform.DOLocalRotate(new Vector3(attackRotationXBack, attackRotationY, 0f), attackAnimationHalfDuration);
    }
}
