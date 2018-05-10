using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWell : MonoBehaviour
{
    private Collider2D myCollider;
    private int bulletCount = 0;
    public GameObject voidBullet;
    public AudioSource bulletExplosionSound, bulletExplosionSound2;
    private bool fireBullets;
    public float fireInterval = 0.15f;
    private float fireIntervalDefault;
    public float numberOfBullets = 50;
    public int timesToFire = 3;
    private int timesToFireDefault;
    public ParticleSystem voidParticles;
    public bool randomDeltaRotation = false;
    public float deltaDeltaRotation = 2f;
    private float deltaRotation = 0f;

    void Start()
    {
        myCollider = GetComponent<CircleCollider2D>();
        fireIntervalDefault = fireInterval;
        fireInterval = 0;
        timesToFireDefault = timesToFire;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            //print("Adding bullet");
            Physics2D.IgnoreCollision(other, myCollider);
            other.gameObject.AddComponent<BulletWellSuckEffect>();
            BulletCount++;
        }
    }

    void Update()
    {
        if (fireBullets && fireInterval <= 0)
        {
            if (timesToFire > 0)
            {
                if (randomDeltaRotation)
                {
                    deltaRotation = Random.Range(0, 360);
                }
                else
                {
                    deltaRotation += deltaDeltaRotation;
                }

                for (int i = 0; i < numberOfBullets; i++)
                {
                    float rotation = 360 / numberOfBullets * i;
                    GameObject newBullet = Instantiate(voidBullet, transform.position, Quaternion.Euler(0, 0, rotation + deltaRotation));
                    newBullet.GetComponent<BulletBase>().StartMoving();
                    newBullet.GetComponent<BulletBase>().playerIndex = 5;
                }

                //if (timesToFire % 8 == 0)
                //    bulletExplosionSound.Play();

                bulletExplosionSound2.pitch = Random.Range(2.7f, 3f);//Random.Range(0.047f, 0.053f);
                bulletExplosionSound2.Play();

                timesToFire--;
            }
            else
            {
                timesToFire = timesToFireDefault;
                fireBullets = false;
                fireInterval = 0;
                bulletExplosionSound.Play();
            }

            fireInterval = fireIntervalDefault;
        }
        else
        {
            fireInterval -= Time.deltaTime;
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fireBullets = true;

            bulletCount = 0;

            bulletExplosionSound.Play();
        }
    }

    private int BulletCount
    {
        get
        {
            return bulletCount;
        }

        set
        {
            bulletCount = value;

            if (bulletCount >= 50)
            {
                fireBullets = true;

                bulletCount = 0;

                bulletExplosionSound.Play();
            }

            if (bulletCount < 30)
            {
                ParticleSystem.EmissionModule em = voidParticles.emission;
                em.rateOverTime = bulletCount / 3;
            }
            else
            {
                ParticleSystem.EmissionModule em = voidParticles.emission;
                em.rateOverTime = bulletCount / 3 + (bulletCount - 30) * 40;
            }
        }
    }
}
