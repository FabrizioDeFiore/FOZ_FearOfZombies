using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public override void EnterState (ActionStateManager actions){
        //actions.rHandAim.weight = 1;
        //actions.lHandIK.weight = 1;
    }
    public override void UpdateState (ActionStateManager actions){
        // Change the weight of the rig constraints smoothly when finish reloading 
        actions.rHandAim.weight = Mathf.Lerp(actions.rHandAim.weight, 1, 10 * Time.deltaTime);
        actions.lHandIK.weight = Mathf.Lerp(actions.lHandIK.weight, 1, 10 * Time.deltaTime);
        // Reload weapon
        if (Input.GetKeyDown(KeyCode.R) && CanReload(actions)){
            actions.SwitchState(actions.Reload);
        }
    }

    bool CanReload(ActionStateManager action){
        // Prevent from reloading if the clip is full or there is no extra ammo
        if (action.ammo.currentAmmo == action.ammo.clipSize) return false;
        else if (action.ammo.extraAmmo == 0) return false;
        else return true;
    }
}
