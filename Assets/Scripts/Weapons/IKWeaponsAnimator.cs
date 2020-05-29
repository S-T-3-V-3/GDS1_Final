using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKWeaponsAnimator : MonoBehaviour
{
    Transform rightHandPosition;
    Transform leftHandPosition;
    Animator playerAnimator;
    float IKweight = 1f;

    bool isEquiping = true;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void SetWeaponHandIK(WeaponTransforms weaponTransforms, Transform gunPosition)
    {
        isEquiping = true;

        rightHandPosition = weaponTransforms.rightHandPoint;
        leftHandPosition = weaponTransforms.leftHandPoint;
        weaponTransforms.transform.parent = transform;
        weaponTransforms.transform.position = gunPosition.position;

        isEquiping = false;
    }

    //change rotation of hand
    private void OnAnimatorIK(int layerIndex)
    {
        if (isEquiping) return;

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKweight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKweight);
        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPosition.position + Vector3.up);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandPosition.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKweight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKweight);
        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPosition.position + Vector3.up);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPosition.rotation);
    }
}
