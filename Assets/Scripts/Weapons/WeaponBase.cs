using UnityEngine;
using System.Collections;

public class WeaponBase : MonoBehaviour
{
    //Time between bullets in seconds
    public float fireRate;
    protected float fireInterval = 0;

    //Number of bullets spawned each shot
    [Tooltip("The number of bullets the gun shoots out every time it fires.")]
    public int bulletAmount;

    //The amount of ammo the weapon has on pickup. 0 is infinite
    [Tooltip("The amount of ammo the weapon has on pickup. 0 is infinite")]
    public int ammoAmount;
    protected int curAmmo;

    //The spread of the gun in degrees. 0 is none
    [Tooltip("The spread of the gun in degrees. 0 is none")]
    public float spread;

    // The amount bullets deviate from where the player is aiming
    [Tooltip("The amount bullets deviate from where the player is aiming in degrees")]
    public float bulletDeviation;

    // The maximum time in seconds the bullet will live after shot. 0 is infinite
    [Tooltip("The maximum time in seconds the bullet will live after shot. 0 is infinite")]
    public float bulletLifetime;

    // Prefab for the bullets from the weapon
    public GameObject bulletPrefab;

    // Spawnpoints for the bullets
    [Tooltip("The position where the bullet spawns")]
    public Transform bulletSpawn;

    // shooting sounds
    private AudioSource source;
    public float pitchMin = 1.0f, pitchMax = 1.0f;

    public bool showReloadBar = true;
    private ReloadBar reloadBar;

    protected GameObject currentPlayer;

    private WeaponBehaviour _shootingBehaviour;
    /// <summary>
    /// The shooting behaviour for this weapon.
    /// </summary>
    public WeaponBehaviour ShootingBehaviour
    {
        get
        {
            return _shootingBehaviour;
        }

        set
        {
            _shootingBehaviour = value;
            _shootingBehaviour.Stats = new WeaponBehaviour.WeaponStats(bulletSpeed, bulletDamage, bulletColor);
        }
    }

    // Weapon variable stuff
    [HideInInspector]
    public int bulletDamage;
    [HideInInspector]
    public float bulletSpeed;
    [HideInInspector]
    public Color bulletColor;

    public virtual void Start()
    {
        source = GetComponent<AudioSource>();
        curAmmo = ammoAmount;
        reloadBar = this.GetComponentInParent<ReloadBar>();
        currentPlayer = this.transform.parent.parent.gameObject;
    }
    
    public virtual void Update()
    {        
        FireIntervalCountdown();
    }

    void FireIntervalCountdown()
    {
        if(fireInterval > 0)
        {
            fireInterval -= Time.deltaTime;

            if(showReloadBar)
            {
                reloadBar.SetEnabled(true);
                reloadBar.UpdateReloadBar(fireInterval, fireRate);
            }
        }
        else
        {
            reloadBar.SetEnabled(false);
        }
    }

    public virtual void Fire()
    {
        if (fireInterval <= 0)
        {
            Shoot();

            HandleSound();

            fireInterval += fireRate;

            HandleAmmo();
        }
    }

    protected IEnumerator emptyWeapon()
    {
        yield return new WaitForSeconds(fireRate);
        currentPlayer.GetComponent<LocalPlayerController>().GiveDefaultWeapon();
    }

    protected virtual void Shoot()
    {
        float angle = Random.Range(-bulletDeviation, bulletDeviation);
        float deltaAngle = spread / (bulletAmount + 1);

        for (int i = 1; i <= bulletAmount; i++)
        {
            Vector3 rot = bulletSpawn.rotation.eulerAngles;
            rot.z += angle + spread / 2f - (deltaAngle * i);

            // Execute shooting behaviour.
            _shootingBehaviour.Activate(bulletSpawn.position, Quaternion.Euler(rot));

            ////////// OLD SPAWN CODE //////////

            // GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.Euler(rot)) as GameObject;

            // // Check if the assigned bullet is a bullet itself or a bullet container
            // if (bullet.GetComponent<BulletBase>() == null)
            // {
            //     foreach (BulletBase childBullet in bullet.GetComponentsInChildren<BulletBase>())
            //     {
            //         SpawnBullet(childBullet.gameObject);
            //     }
            // }
            // else
            // {
            //     SpawnBullet(bullet);
            // }
        }
    }

    private void SpawnBullet(GameObject bullet)
    {
        BulletBase bulletBase = bullet.GetComponent<BulletBase>();
        bulletBase.bulletSpeed = this.bulletSpeed;
        bulletBase.GetComponent<SpriteRenderer>().color = bulletColor; 
        
        bulletBase.damage = bulletDamage;
        bulletBase.StartMoving();
        bulletBase.playerIndex = currentPlayer.GetComponent<LocalPlayerController>().playerIndex;

        if (bulletLifetime > 0)
            Destroy(bullet, bulletLifetime);
    }

    protected virtual void HandleAmmo()
    { 
        if (ammoAmount == 0)
        {
            return;
        }

        curAmmo--;
        if (curAmmo <= 0)
        {
            StartCoroutine(emptyWeapon());
        }
    }

    protected virtual void HandleSound()
    {
        // Play sound
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();
    }
}
