using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] Transform recoilFollowPosition;
    [SerializeField] float kickBackAmount = -1;
    [SerializeField] float kickBackSpeed = 10, returnSpeed = 20;
    float currentRecoilPosition, finalRecoilPosition;

    // Update is called once per frame
    void Update() {
        // Calculate the recoil position
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);
        // Update recoil position
        recoilFollowPosition.localPosition = new Vector3(0, 0, finalRecoilPosition);
    }

    // Apply the recoil to the weapon
    public void TriggerRecoil() => currentRecoilPosition += kickBackAmount;
        

}
