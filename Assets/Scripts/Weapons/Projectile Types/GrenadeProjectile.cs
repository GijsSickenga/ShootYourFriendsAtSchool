using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : DefaultProjectile
{
    // Hier alle code voor het afhandelen van grenade
    // Call zelf ergens OnTriggerBehaviour (kijk in base class)

    // Mss deze laten overerven van DefaultProjectile
    // en overriden waar OnTriggerBehaviour aangeroepen wordt

    private float _currentSpeed;
    private const float EXPLOSION_TIMER_START = 2f;
    private float _explosionTimer = EXPLOSION_TIMER_START;
    
    protected override void Update()
    {
        base.Update();

        // Increment explosion timer.
        _explosionTimer -= Time.deltaTime;

        // Explode when timer is finished.
        if (_explosionTimer < 0)
        {
            Explode();
        }

        // Slow grenade over time.
        _currentSpeed = (Stats._projectileSpeed / 2) * ((_explosionTimer / EXPLOSION_TIMER_START) + (EXPLOSION_TIMER_START / 10));
        projectileRigidBody.velocity = transform.right * _currentSpeed;
    }

    protected virtual void Explode()
    {
        if (OnTriggerBehaviour != null)
        {
            OnTriggerBehaviour(transform.position, transform.rotation.eulerAngles, this);
        }
        SpawnParticleSystem(null);   
        Destroy(gameObject);
    }

    protected override void HandleDamage(Collider2D other)
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

            Destroy(gameObject);
        }
        else if (breakHealth != null)
        {
            breakHealth.Damage(Stats._projectileDamage);

            Destroy(gameObject);
        }
        else
        {
            // Nothing with health hit
            RaycastHit2D rayCast;
            Vector2 vel = GetComponent<Rigidbody2D>().velocity;
            rayCast = Physics2D.Raycast(transform.position, vel);

            Vector2 reflected = Vector2.Reflect(vel, rayCast.normal);
            reflected.Normalize();
            float angle = Mathf.Atan2(reflected.y, reflected.x);

            Vector2 newPos = rayCast.point;
            Vector3 newAngle = new Vector3(0, 0, Mathf.Rad2Deg * angle);

            // Debug.DrawLine(newPos, reflected * 1 + newPos, Color.red, 2);
            // Debug.DrawLine(newPos, -vel.normalized * 1 + newPos, Color.red, 2);

            // Don't trigger behaviour, instead bounce.
            transform.position = newPos;
            transform.rotation = Quaternion.Euler(newAngle);
            projectileRigidBody.velocity = transform.right * _currentSpeed;
        }
    }
}
