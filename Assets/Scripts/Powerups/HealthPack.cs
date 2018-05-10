using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

    public float healAmount = 40f;
    public GameObject ps;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Heal(healAmount);
            Destroy(gameObject);
            GameObject particleSystem = Instantiate(ps, transform.position, Quaternion.identity) as GameObject;
            Destroy(particleSystem, 3.0f);
        }
    }
}
