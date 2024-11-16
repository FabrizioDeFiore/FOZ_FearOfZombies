using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;
    public float spawnDelay = 0.5f;
    public float currentWave = 0;
    public float waveCooldown = 10f;
    public bool inCooldown;
    public float cooldownCounter = 0;
    public List<GameObject> currentZombiesAlive;
    List<GameObject> zombiesToDestroy = new List<GameObject>();
    public GameObject zombiePrefab;
    public string meshFolderPath = "Meshes/ZombieMeshes"; 
    private Mesh[] zombieMeshes; 
    private List<GameObject> zombies = new List<GameObject>(); 

    public TextMeshProUGUI waveToStartText;
    public TextMeshProUGUI coolDownCounterText;
    public TextMeshProUGUI zombiesAliveText;
    public TextMeshProUGUI roundsCounterText;
    // Start is called before the first frame update
    void Start(){
        // Load all meshes from the specified folder
        zombieMeshes = Resources.LoadAll<Mesh>(meshFolderPath);
        // Set the initial number of zombies per wave
        currentZombiesPerWave = initialZombiesPerWave;
        StarNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Get all dead zombies
        List<GameObject> zombiesToRemove = new List<GameObject>();
        foreach (GameObject zombie in currentZombiesAlive){
            // Check if the zombie is dead and add it to the new list
            if (zombie.GetComponent<ZombieHealth>().health <= 0){
                zombiesToRemove.Add(zombie);
            }
        }
        // Remove all dead zombies in level from the list 
        // (Doing this double list iteration to avoid errors when
        // deleting elements from the list while iterating through it)
        foreach (GameObject zombie in zombiesToRemove){
            currentZombiesAlive.Remove(zombie);
            // Add the zombie to the list of zombies to destroy to destroy the dead body at the start of the new round
            zombiesToDestroy.Add(zombie);
        }
        // Reset the list
        zombiesToRemove.Clear();
        //When all zombies are dead, start the cooldown
        if (currentZombiesAlive.Count == 0 && inCooldown == false){
            // Start the coroutine for the next wave
            StartCoroutine(WaveCooldown());
        }
        // Run the cooldown counter
        if (inCooldown){
            cooldownCounter -= Time.deltaTime;
        } else {
            cooldownCounter = waveCooldown;
        }
        // Update the UI
        coolDownCounterText.text = cooldownCounter.ToString("F0");
        zombiesAliveText.text = "Zombies: " + currentZombiesAlive.Count.ToString() + "/" + currentZombiesPerWave.ToString();
    }

    public void StarNextWave(){
        // Clear all object from the list 
        currentZombiesAlive.Clear();
        // Update wave counter
        if (currentZombiesPerWave == 5) currentWave = 1;
        else currentWave++;
        // Update the UI
        if (currentWave < 10) roundsCounterText.text = "0" + currentWave.ToString();
        else roundsCounterText.text = currentWave.ToString();
        // Start the coroutine to spawn zombies
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave(){
        // Loop through the number of zombies to spawn
        for (int i = 0; i < currentZombiesPerWave; i++){
            // Generate a random offset within a specific range
            Vector3 spawnOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 spawnPos = transform.position + spawnOffset;
            // Spawn a zombie
            GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
            // Assign a random mesh to the zombie from my folder 
            Mesh randomMesh = zombieMeshes[Random.Range(0, zombieMeshes.Length)];
            SkinnedMeshRenderer meshFilter = zombie.GetComponentInChildren<SkinnedMeshRenderer>();
            meshFilter.sharedMesh = randomMesh;
            // Add the zombie to the list
            currentZombiesAlive.Add(zombie);
            // Wait for the delay
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator WaveCooldown(){
        // Set the cooldown flag on and wait for the cooldown time
        inCooldown = true;
        // Display cooldown ui
        waveToStartText.gameObject.SetActive(true);
        waveToStartText.text = "Round " + (currentWave+1) + " will start in:";
        coolDownCounterText.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCooldown);
        // Set the cooldown flag off 
        inCooldown = false;
        // Destroy all dead zombies body from the previous wave
        foreach (GameObject zombie in zombiesToDestroy){
            Destroy(zombie);
        }
        // Hide cooldown ui
        waveToStartText.gameObject.SetActive(false);
        coolDownCounterText.gameObject.SetActive(false);
        // Incrrase the number of zombies per wave
        currentZombiesPerWave += 2;
        // Start the next wave
        StarNextWave();
    }
}
