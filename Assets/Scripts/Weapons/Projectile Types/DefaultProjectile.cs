using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : BehaviourProjectile
{
    // Hier alle code voor het afhandelen van de default bullet
    // Call zelf ergens OnTriggerBehaviour (kijk in base class)

    // Wss de hele bulletbase copy/pasta

    public GameObject hitParticleSystem;

    void Start()
    {
        PlayerID = PlayerID;
        GetComponent<SpriteRenderer>().color = Stats._projectileColor; 
        StartMoving();
    }

    public virtual void StartMoving()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * Stats._projectileSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    void Update()
    {
        Vector2 direction = (GetComponent<Rigidbody2D>().velocity).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    protected virtual void HandleCollision(Collider2D other)
    {
        foreach (string tag in GlobalVariables.IgnoreCollisionTags)
        {
            if (other.CompareTag(tag))
            {
                return;
            }
        }

        if (other.CompareTag("Player"))
        {
            if (PlayerID == other.GetComponent<LocalPlayerController>().playerIndex)
            {
                return;
            }
        }

        HandleDamage(other);

        SpawnParticleSystem(other);
    }

    protected virtual void HandleDamage(Collider2D other)
    {
        GameObject hit = other.gameObject;
        Health health = hit.GetComponent<Health>();
        Breakable breakHealth = hit.GetComponent<Breakable>();

        if (health != null)
        {
            //Giving lastHit the PlayerID of the shooting player
            health.lastHit = PlayerID;
            health.ResetLastHitTimer();
            health.Damage(Stats._projectileDamage);
        }
        else if (breakHealth != null)
        {
            breakHealth.Damage(Stats._projectileDamage);
        }
        else
        {
            // Nothing with health hit
            RaycastHit2D rayCast;
            Vector2 vel = GetComponent<Rigidbody2D>().velocity;
            rayCast = Physics2D.Raycast(transform.position, vel);
            
            Vector2 reflected = Vector2.Reflect(vel, rayCast.normal);
            reflected.Normalize();
            float angle = Mathf.Tan(reflected.y / reflected.x);

            OnTriggerBehaviour(rayCast.point, new Vector3(0, 0, Mathf.Rad2Deg * angle));
        }

        Destroy(gameObject);
    }

    protected virtual void SpawnParticleSystem(Collider2D other)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, GetComponent<Rigidbody2D>().velocity);

        GameObject ps = Instantiate(hitParticleSystem, hit.point, Quaternion.LookRotation(Vector3.forward, hit.normal)) as GameObject;
        ps.transform.Rotate(0, 0, 90);
        Destroy(ps, 2);
    }
}
