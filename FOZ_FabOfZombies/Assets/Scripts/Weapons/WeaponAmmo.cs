using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WeaponAmmo : MonoBehaviour
{
    public int clipSize;
    public int extraAmmo;
    public int maxAmmo = 500;
    [HideInInspector] public int currentAmmo; //[HideInInspector]#
    AudioSource audioSource;
    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip slideReleaseSound;
    public AudioClip buySound;
    public AudioClip notEnoughMoneySound;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI buyAmmoText;
    public TextMeshProUGUI moneyText;
    PlayerManager playerManager;
    public Transform ammoBox;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = clipSize;
        playerManager = GetComponentInParent<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        BuyAmmo();
        if (Input.GetKeyDown(KeyCode.R)) Reload();
        // Update UI
        ammoText.text = currentAmmo + "/" + extraAmmo;
        moneyText.text = playerManager.money.ToString();

        
    }

    void BuyAmmo(){
        // Check if player is near the ammo box
        if (Vector3.Distance(transform.position, ammoBox.transform.position) < 2 && (currentAmmo != clipSize || extraAmmo != maxAmmo) ){
            // Display buy ammo UI  
            buyAmmoText.gameObject.SetActive(true);
            // Check if pressing the buy button
            if (Input.GetKeyDown(KeyCode.E)){
                // Check if player has enough money
                if (playerManager.money >= 100){
                    // Play buy sound
                    audioSource.PlayOneShot(buySound);
                    // Update ammo values
                    extraAmmo = maxAmmo;
                    currentAmmo = clipSize;
                    // Update money value
                    playerManager.money -= 100;
                }else {
                    // Play not enough money sound
                    audioSource.PlayOneShot(notEnoughMoneySound);
                }
            }
        } else {
            // Hide buy ammo UI
            buyAmmoText.gameObject.SetActive(false);
        }
    }

    public void Reload(){
        // Check if there is enough ammo to reload a full clip
        if (extraAmmo >= clipSize){
            // Get the amount of ammo needed to fill the clip
            int ammoToReaload = clipSize - currentAmmo;
            // Update ammo values
            extraAmmo -= ammoToReaload;
            currentAmmo += ammoToReaload;
        }
        // 
        else if (extraAmmo > 0){
            // Check if ammos are gonna exceed the clip size
            if (extraAmmo + currentAmmo > clipSize){
                // Get the amount of ammo left after filling the clip
                int leftOverAmmo = extraAmmo + currentAmmo - clipSize;
                extraAmmo = leftOverAmmo;
                // Fill the clip
                currentAmmo = clipSize;
            }
            else{
                // Fill the clip with the remaining ammo
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
        }
        
    }
    

}
