using UnityEngine;
using System.Collections;

public class WeaponSpawn : MonoBehaviour {
    public GameObject weaponPickUp;

    public float maxSpawnTimer;

    public enum SpawnSelection
    {
        Random, Cycle
    }
    public SpawnSelection spawnSelection;

    public GameObject[] weapons;

    private float spawnTimer;
    private GameObject currentSpawnedWeapon;
    private int index;

    void Start()
    {
        spawnTimer = 0;
        index = 0;
    }

    void FixedUpdate()
    {
        if(currentSpawnedWeapon == null)
        {
            spawnTimer -= Time.deltaTime;

            if(spawnTimer <= 0)
            {
                switch(spawnSelection)
                {
                    case SpawnSelection.Random:
                        SpawnRandomWeapon();
                        break;
                    case SpawnSelection.Cycle:
                        SpawnCycledWeapon();
                        break;
                    default:
                        SpawnRandomWeapon();
                        break;
                }
                
            }
        }
    }

    private void SpawnRandomWeapon()
    {
        GameObject gun = Instantiate(weaponPickUp, transform.position, Quaternion.identity) as GameObject;
        gun.GetComponent<WeaponPickUpController>().weapon = weapons[Random.Range(0, weapons.Length)];
        currentSpawnedWeapon = gun;
        ResetTimer();
    }

    private void SpawnCycledWeapon()
    {
        GameObject gun = Instantiate(weaponPickUp, transform.position, Quaternion.identity) as GameObject;
        gun.GetComponent<WeaponPickUpController>().weapon = weapons[index];
        currentSpawnedWeapon = gun;

        ResetTimer();

        index++;
        if(index >= weapons.Length)
        {
            index = 0;
        }
    }

    private void ResetTimer()
    {
        spawnTimer = maxSpawnTimer;
    }
}
