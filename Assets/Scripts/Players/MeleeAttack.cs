using UnityEngine;
using System.Collections;
using System;

public class MeleeAttack : MonoBehaviour
{
    public float rad = 1.4f;
    public float maxAngle = 90.0f;

    public float meleeDelay = 3f;
    private float meleeTimer;

    public float stunLength = 0.3f;
    
    public float liniearDrag = 15.0f;
    public float meleeForce = 1000.0f;

    public float damage = 35.0f;

    private Transform pivot;

    public GameObject sword;
    private SpriteAnimationManager swordAnimations;
    public AudioSource swordSwingSound;
    public float pitchMin = 1.0f, pitchMax = 1.0f;

    private int layerMask;

    void Start()
    {
        meleeTimer = 0.0f;
        pivot = GetComponent<LocalPlayerController>().pivot;

        swordAnimations = sword.GetComponent<SpriteAnimationManager>();
        SpriteRenderer swordSprite = sword.GetComponent<SpriteRenderer>();
        sword.transform.localScale = new Vector3(rad / swordSprite.bounds.size.x * 2, rad / swordSprite.bounds.size.y * 2, 0.0f);

        layerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Ignore Raycast"));
    }

	void Update () {
        if (meleeTimer > 0)
        {
            meleeTimer -= Time.deltaTime;
        }
    }

    public void TryMelee()
    {
        if(meleeTimer <= 0)
        {
            Melee();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 pos = transform.position;

        // Draw circle for range
        Gizmos.DrawWireSphere(pos, rad);

        // Position of range from Vector3.zero
        Vector3 toPos = pivot.right * rad;
        // Clone vector for second position
        Vector3 toPos2 = new Vector3(toPos.x, toPos.y, toPos.z);
        
        // Rotate the vector maxAngle around vector3.zero, both ways
        toPos = Quaternion.Euler(0, 0, maxAngle) * toPos;
        toPos2 = Quaternion.Euler(0, 0, -maxAngle) * toPos2;

        // Place vector at right position compared to player
        toPos += transform.position;
        toPos2 += transform.position;

        Gizmos.DrawLine(pos, toPos);
        Gizmos.DrawLine(pos, toPos2);
    }

    private void Melee()
    {
        //Play animation
        sword.SetActive(true);
        swordAnimations.Play("SwordSwing");

        // Play sound
        swordSwingSound.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
        swordSwingSound.Play();

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, rad, layerMask);
        
        foreach (Collider2D col in cols)
        {
            if ((col.CompareTag("BreakableWall") || col.CompareTag("Player") || col.CompareTag("Bullet")) && col.gameObject != this.gameObject)
            {
                // Compare angle of the hit to the angle of the player with the maximun angle of the melee
                Vector2 dir = (col.transform.position - transform.position).normalized;
                float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion quaternionRotation = Quaternion.Euler(0, 0, rot);
                float angle = Quaternion.Angle(pivot.rotation, quaternionRotation);
                
                if (angle < maxAngle)
                {
                    // Raycast to other player (from bulletspawn to prevent the ray hitting the player) to check if there are no obstacles between the two players
                    Vector3 bulletSpawn = GetComponent<LocalPlayerController>().GetCurrentWeapon().GetComponent<WeaponBase>().bulletSpawn.position;
                    RaycastHit2D hit = Physics2D.Raycast(bulletSpawn, col.transform.position - bulletSpawn, Mathf.Infinity, layerMask);

                    if (hit.collider.gameObject != col.gameObject)
                    {
                        continue;
                    }
                    
                    switch (col.tag)
                    {
                        case "Player":
                            StartCoroutine(col.GetComponent<LocalPlayerController>().StunForSeconds(stunLength, liniearDrag));

                            // Apply force in the right direction
                            col.GetComponent<Rigidbody2D>().AddForce(dir * meleeForce);

                            //Damage other player
                            col.GetComponent<Health>().lastHit = GetComponent<LocalPlayerController>().playerIndex;
                            col.GetComponent<Health>().Damage(damage);
                            break;
                        case "BreakableWall":
                            col.GetComponent<Breakable>().Damage((int)damage);
                            break;
                        case "Bullet":
                            // Change velocity of the bullet in the direction defined ealier
                            Vector2 vel = col.gameObject.GetComponent<Rigidbody2D>().velocity;
                            vel = dir * vel.magnitude;
                            col.gameObject.GetComponent<Rigidbody2D>().velocity = vel;

                            // Change the source index of the bullet
                            col.GetComponent<BulletBase>().playerIndex = GetComponent<LocalPlayerController>().playerIndex;

                            // Change rotation of the bullet sprite
                            float rotation = Mathf.Atan2(vel.y, vel.x);
                            col.transform.rotation = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // Reset timer
        meleeTimer = meleeDelay;
    }
}
