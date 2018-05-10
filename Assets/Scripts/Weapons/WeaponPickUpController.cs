using UnityEngine;
using System.Collections;

public class WeaponPickUpController : MonoBehaviour {
    public GameObject weapon;
	
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        this.GetComponent<SpriteRenderer>().color = weapon.GetComponent<SpriteRenderer>().color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<LocalPlayerController>().GiveWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
