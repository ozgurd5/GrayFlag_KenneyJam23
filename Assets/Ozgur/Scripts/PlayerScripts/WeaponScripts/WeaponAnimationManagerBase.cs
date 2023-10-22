using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAnimationManagerBase : MonoBehaviour
{
    protected Tweener attackRotationTween;
    protected Slider cooldownSlider;
    protected bool isAttackAnimationPlaying;

    protected IEnumerator PlayAttackAnimation(Transform weaponTransform, float attackRotationX, float attackRotationXBack, float attackRotationY, float attackAnimationHalfDuration)
    {
        isAttackAnimationPlaying = true;

        attackRotationTween = weaponTransform.transform.DOLocalRotate(new Vector3(attackRotationX, attackRotationY, 0f), attackAnimationHalfDuration);
        DOTween.To(() => cooldownSlider.value, value => cooldownSlider.value = value, 0, attackAnimationHalfDuration);
        yield return new WaitForSeconds(attackAnimationHalfDuration);

        attackRotationTween = weaponTransform.transform.DOLocalRotate(new Vector3(attackRotationXBack, attackRotationY, 0f), attackAnimationHalfDuration);
        DOTween.To(() => cooldownSlider.value, value => cooldownSlider.value = value, 1, attackAnimationHalfDuration);
        yield return new WaitForSeconds(attackAnimationHalfDuration);

        isAttackAnimationPlaying = false;
    }
}
