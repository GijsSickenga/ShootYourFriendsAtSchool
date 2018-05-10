using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {
    private List<GameObject> spawnPoints;
    private int spawnIndex;
    public int spawnTimer = 2;
    public GameObject particleSys;

    void Start()
    {
        spawnPoints = new List<GameObject>();

        foreach(GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoints.Add(spawnPoint.gameObject);
        }

        if(spawnPoints.Count <= 0)
        {
            Debug.Log("No spawnpoints found!");
        }

        spawnIndex = 0;
    }
    
    public GameObject PickRandomRespawn()
    {
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        return spawnPoint;
    }

    public GameObject PickNextRespawn()
    {
        GameObject spawnPoint = spawnPoints[spawnIndex];
        spawnIndex++;

        if(spawnIndex >= spawnPoints.Count)
        {
            spawnIndex = 0;
        }

        return spawnPoint;
    }

    public void Respawn(GameObject player)
    {
        StartCoroutine(RespawnPlayer(player));
    }

    public IEnumerator RespawnPlayer(GameObject player)
    {
        player.SetActive(false);

        yield return new WaitForSeconds(spawnTimer);

        player.SetActive(true);
        player.transform.position = PickRandomRespawn().transform.position;
        player.GetComponent<LocalPlayerController>().GiveDefaultWeapon();
        Health health = player.GetComponent<Health>();
        health.SetHealth(health.maxHealth);

        createParticleSystem(player);
    }

    public void createParticleSystem(GameObject player)
    {
        GameObject ps = Instantiate(particleSys, player.transform.position, player.transform.rotation) as GameObject;
        ps.GetComponent<ParticleSystem>().startColor = ColorTracker.colors[(int)player.GetComponent<LocalPlayerController>().pIndex];
        Destroy(ps, 3);
    }
}
