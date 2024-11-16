using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;
    //public Image helthBarFill;
    public Slider healthBar;
    public GameObject gameOverScreen;
    public AudioClip gameOverSound;
    AudioSource audioSource;

    private void Start(){
        audioSource = GetComponent<AudioSource>();
        //healthBar = GetComponent<Slider>();
        if (healthBar == null){
            Debug.Log("Health NOT Bar Found");
        }
        healthBar.maxValue = health;
    }
    public void TakeDamage(float damage){
        if (health > 0){
            health -= damage;
            //bool isVulnerable = false;
        }
        if (health <= 0) PlayerDeath();
    }
    
    private IEnumerator DamageCooldown(Collider zombiehand){
        // Disable the zombie's hand 
        zombiehand.enabled = false;
        // Wait for 1 second
        yield return new WaitForSeconds(1.0f); 
        // Re-enable the zombie's hand collider
        zombiehand.enabled = true;

    }

    void PlayerDeath(){
        Debug.Log("Player Dead");
        // Show Game over screen 
        gameOverScreen.SetActive(true);
        // Play game over sound
        audioSource.PlayOneShot(gameOverSound);
        // Stop the game
        Time.timeScale = 0;

    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("ZombieHand")){    
            TakeDamage(other.gameObject.GetComponentInParent<ZombieManager>().damage);
            // Set up a small cooldown to prevent the player from taking more than one damage in one hit
            StartCoroutine(DamageCooldown(other));
        }
    }

    private void Update(){
        //helthBarFill.fillAmount = health / 100;
        healthBar.value = health;
        Debug.Log("healthBar.value: " + healthBar.value);
        Debug.Log("Health: " + health);
        //Debug.Log("Fill Amount: " + helthBarFill.fillAmount);
    }
}
