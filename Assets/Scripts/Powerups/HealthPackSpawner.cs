using UnityEngine;
using System.Collections;

public class HealthPackSpawner : MonoBehaviour {
    public GameObject healthPackPrefab;

    public float healthSpawnTime = 10.0f;
    private float healthSpawnTimer;

    private GameObject healthPackInstance;

    void Start()
    {
        ResetTimer();
    }

    void FixedUpdate()
    {
        if(healthPackInstance == null)
        {
            healthSpawnTimer -= Time.fixedDeltaTime;

            if(healthSpawnTimer <= 0)
            {
                healthPackInstance = Instantiate(healthPackPrefab, transform.position, Quaternion.identity) as GameObject;
                ResetTimer();
            }
        }
    }

    public void ResetTimer()
    {
        healthSpawnTimer = healthSpawnTime;
    }
}
