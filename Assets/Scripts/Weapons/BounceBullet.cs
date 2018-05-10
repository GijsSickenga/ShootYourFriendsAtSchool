using UnityEngine;
using System.Collections;

public class BounceBullet : BulletBase {

    private AudioSource source;

    //Maximum number of bounces
    public int maxNumOfBounces = 4;

    private int numOfBounces;

    void Start()
    {
        source = GetComponent<AudioSource>();
        numOfBounces = 0;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        this.GetComponent<Collider2D>().isTrigger = true;
    }

    protected override void HandleDamage(Collider2D other)
    {

        GameObject hit = other.gameObject;
        Health health = hit.GetComponent<Health>();
        Breakable breakHealth = hit.GetComponent<Breakable>();

        if (health != null)
        {
            health.Damage(damage);
            health.lastHit = playerIndex;
            Destroy(gameObject);
        }
        else if (breakHealth != null)
        {
            breakHealth.Damage(damage);
            Destroy(gameObject);
        }

        this.GetComponent<Collider2D>().isTrigger = false;

        source.Play();
        numOfBounces++;

        if (numOfBounces >= maxNumOfBounces)
        {
            Destroy(gameObject);
        }
    }
}
