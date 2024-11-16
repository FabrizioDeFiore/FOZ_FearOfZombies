using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{

    [HideInInspector] public ActionBaseState currentState;
    public ReloadState Reload = new ReloadState();
    public DefaultState Default = new DefaultState();

    public GameObject currentWeapon;
    [HideInInspector] public WeaponAmmo ammo;
    AudioSource audioSource;

    [HideInInspector] public Animator anim;
    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ammo = currentWeapon.GetComponent<WeaponAmmo>();
        audioSource = currentWeapon.GetComponent<AudioSource>();
        SwitchState(Default);

    }

    // Update is called once per frame
    void Update(){
        currentState.UpdateState(this);
    }
    public void SwitchState(ActionBaseState state){
        currentState = state;
        currentState.EnterState(this);
    }

    public void WeaponReloaded(){
        // Update ammo values
        ammo.Reload();
        // rHandAim.weight = 1;
        // lHandIK.weight = 1;
        SwitchState(Default);
    }

    public void MagOut(){
        audioSource.PlayOneShot(ammo.magOutSound);
    }

    public void MagIn(){
        audioSource.PlayOneShot(ammo.magInSound);
    }

    public void ReleaseSlide(){
        audioSource.PlayOneShot(ammo.slideReleaseSound);
    }
}
