using UnityEngine;
using System.Collections;

public class ArrowTrap : MonoBehaviour
{
    // Time between bullets in seconds
    public float fireRate;
    protected float fireInterval = 0;

    // How long it takes for the trap to reload
    public float reloadTimer;
    private float reloadTimerDefault;

    // How many times the trap shoots in its shooting state before reloading
    public int timesToShoot;
    private int timesToShootDefault;

    // Number of projectiles spawned each shot
    public int projectileAmount;

    // The spread of the trap in degrees. 0 is none
    [Range(0, 360)]
    public float spread;

    // The amount in degrees projectiles deviate from where the trap is aiming
    [Range(0, 360)]
    public float projectileDeviation;

    // The maximum time in seconds the projectile will live after shot. 0 is infinite
    public float projectileLifetime;

    // Position to spawn projectiles at
    public Transform projectileSpawn;

    // Projectile prefab containing all projectile movement code
    public GameObject projectilePrefab;

    // Animated sprites
    public Sprite primedSprite, shootSprite, reloadSprite;

    private enum States { Primed, Shoot, Reload };
    States state = States.Primed;

    private SpriteRenderer spriteRenderer;

    // Sound to play when the trap shoots projectiles
    private AudioSource shootSound;

    void Start()
    {
        reloadTimerDefault = reloadTimer;

        timesToShootDefault = timesToShoot;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        shootSound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case States.Primed:
                RaycastHit2D hit = Physics2D.Raycast(projectileSpawn.position, transform.right);
                if (hit.transform.CompareTag("Player"))
                {
                    state = States.Shoot;
                    spriteRenderer.sprite = shootSprite;
                }
                break;

            case States.Shoot:
                if (timesToShoot > 0)
                {
                    fireInterval -= Time.fixedDeltaTime;

                    Fire();
                }
                else
                {
                    state = States.Reload;
                    timesToShoot = timesToShootDefault;
                    spriteRenderer.sprite = reloadSprite;
                    fireInterval = 0;
                }
                break;

            case States.Reload:
                if (reloadTimer <= 0)
                {
                    state = States.Primed;
                    reloadTimer = reloadTimerDefault;
                    spriteRenderer.sprite = primedSprite;
                }
                else
                {
                    reloadTimer -= Time.fixedDeltaTime;
                }
                break;
        }
    }

    void Fire()
    {
        if (fireInterval <= 0)
        {
            float angle = Random.Range(-projectileDeviation, projectileDeviation);

            for (int i = 0; i < projectileAmount; i++)
            {
                float deltaAngle = spread / Mathf.Clamp(((float)projectileAmount - 1), 1, Mathf.Infinity);
                Vector3 rot = projectileSpawn.rotation.eulerAngles;
                rot.z += angle + spread / 2f - (deltaAngle * i);
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.Euler(rot)) as GameObject;

                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), projectile.GetComponent<Collider2D>());

                projectile.GetComponent<BulletBase>().StartMoving();
                // Do something with player index?

                if (projectileLifetime > 0)
                    Destroy(projectile, projectileLifetime);
            }

            // Play sound
            shootSound.pitch = Random.Range(0.9f, 1.1f);
            shootSound.Play();

            fireInterval += fireRate;
            timesToShoot--;
        }
    }
}
