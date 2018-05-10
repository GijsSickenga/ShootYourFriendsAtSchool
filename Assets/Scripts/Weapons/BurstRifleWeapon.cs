using UnityEngine;
using System.Collections;

public class BurstRifleWeapon : WeaponBase
{
    // The delay in seconds between one bullet and the next in a burst
    public float delayPerBullet;
    private float burstFireInterval = 0;

    // The amount of times to fire before the burst ends
    public int firesPerBurst;
    private int remainingShots;

    private bool shooting;

    public override void Start()
    {
        base.Start();
        remainingShots = firesPerBurst;
    }

    void FixedUpdate()
    {
        if (shooting)
        {
            burstFireInterval -= Time.fixedDeltaTime;

            if (burstFireInterval <= 0)
            {
                Shoot();

                HandleSound();

                // Subtract one bullet from the amount of bullets to fire in this burst
                remainingShots--;
                burstFireInterval += delayPerBullet;

                // The burst ends when there are no more remaining shots to be fired
                if (remainingShots <= 0)
                {
                    shooting = false;
                    fireInterval += fireRate;
                    remainingShots = firesPerBurst;
                }

                HandleAmmo();
            }
        }
    }

    public override void Fire()
    {
        if (fireInterval <= 0)
        {
            shooting = true;
        }
    }
}
