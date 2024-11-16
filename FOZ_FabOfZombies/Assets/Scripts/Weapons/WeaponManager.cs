using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{    
    #region Fire Rate
    [Header("Fire Rate")]
    [SerializeField] bool semiAuto;
    [SerializeField] float fireRate;
    float fireRateTimer;
    #endregion

    #region Bullet Properties
    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletPerShot;
    AimStateManager aim;
    WeaponAmmo ammo;
    WeaponBloom bloom;
    public float damage = 20;
    public float zombieKickBackForce = 100;
    public GameObject bloodEffect;
    #endregion

    #region Audio
    [Header("Audio")]
    [SerializeField] AudioClip gunShot;
    AudioSource audioSource;
    ActionStateManager action;
    #endregion

    #region Recoil
    [Header("Recoil")]
    WeaponRecoil recoil;
    Light muzzleFlashLight;
    ParticleSystem muzzleFlashParticles;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        bloom = GetComponent<WeaponBloom>();
        recoil = GetComponent<WeaponRecoil>();
        ammo = GetComponent<WeaponAmmo>();
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        action = GetComponentInParent<ActionStateManager>();
        muzzleFlashLight = GetComponentInChildren<Light>();
        // Reset muzzle flash light intensity
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldFire()) Fire();
        // Update the muzzle flash light
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    bool ShouldFire(){
        // Check if can fire now based on fire rate
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        // Check if there is ammo
        if (ammo.currentAmmo == 0) return false;
        // Check if the weapon is reloading
        if (action.currentState == action.Reload) return false;
        // Check if the player is pressing the fire button
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire(){
        // Reset the fire rate timer
        fireRateTimer = 0;
        // Get the aim direction
        barrelPos.LookAt(aim.aimPos);
        // Get the bloom angle
        barrelPos.localEulerAngles = bloom.BloomAngle(barrelPos);
        // Play the gun shot sound
        audioSource.PlayOneShot(gunShot);
        // Apply recoil
        recoil.TriggerRecoil();
        // Trigger muzzle flash
        TriggerMuzzleFlash();
        // Decrease the ammo
        ammo.currentAmmo--;
        // Fire the bullet
        for (int i = 0; i < bulletPerShot; i++){
            // Create the bullet
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            // Get the bullet script component
            Bullet bulletScript = currentBullet.GetComponent<Bullet>();
            // Set the weapon property of the bullet
            bulletScript.weapon = this;
            // Store bullet direction (to add force to zombie ragdoll when killed on the same dir)
            bulletScript.dir = barrelPos.transform.forward;
            // Add force to the bullet (fire it)
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
            //rb.velocity = barrelPos.forward * bulletVelocity;
        }
    }

    void TriggerMuzzleFlash(){
        // Play particles effect 
        muzzleFlashParticles.Play();
        // Increase the light intensity
        muzzleFlashLight.intensity = lightIntensity;
    }
}
