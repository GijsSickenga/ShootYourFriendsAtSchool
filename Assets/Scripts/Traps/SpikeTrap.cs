using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeTrap : MonoBehaviour
{
    public float warningTimer = 0.75f;
    public float damageTimer = 0.3f;
    public float cooldownTimer = 1.0f;
    [Range(0, 100)]
    public float damageAmount;

    public Sprite primedSprite, warningSprite, damageSprite, cooldownSprite;

    private float warningTimerDefault;
    private float damageTimerDefault;
    private float cooldownTimerDefault;

    private enum States { Primed, Warning, Damage, Cooldown };
    States state = States.Primed;

    private List<GameObject> players = new List<GameObject>();

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        warningTimerDefault = warningTimer;
        damageTimerDefault = damageTimer;
        cooldownTimerDefault = cooldownTimer;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (state)
        {
            case States.Primed:
                if (players.Count > 0)
                {
                    foreach (GameObject player in players)
                    {
                        Vector3 playerPosition = player.transform.position;
                        // Make sure the player is standing at least halfway on top of the trap
                        if (playerPosition.x >= transform.position.x - transform.lossyScale.x / 2f &&
                            playerPosition.x <= transform.position.x + transform.lossyScale.x / 2f &&
                            playerPosition.y >= transform.position.y - transform.lossyScale.y / 2f &&
                            playerPosition.y <= transform.position.y + transform.lossyScale.y / 2f)
                        {
                            state = States.Warning;
                            spriteRenderer.sprite = warningSprite;
                            break;
                        }
                    }
                }
                break;

            case States.Warning:
                warningTimer -= Time.deltaTime;
                if (warningTimer <= 0)
                {
                    state = States.Damage;
                    warningTimer = warningTimerDefault;
                    spriteRenderer.sprite = damageSprite;
                }
                break;

            case States.Damage:
                damageTimer -= Time.deltaTime;
                foreach (GameObject player in players)
                {
                    Health health = player.GetComponent<Health>();
                    if (!health.invulnerableToTraps)
                    {
                        Vector3 playerPosition = player.transform.position;
                        // Make sure the player is standing at least halfway on top of the trap
                        if (playerPosition.x >= transform.position.x - transform.lossyScale.x / 2f &&
                            playerPosition.x <= transform.position.x + transform.lossyScale.x / 2f &&
                            playerPosition.y >= transform.position.y - transform.lossyScale.y / 2f &&
                            playerPosition.y <= transform.position.y + transform.lossyScale.y / 2f)
                        {
                            health.Damage(damageAmount / 100 * health.maxHealth, damageTimer);
                        }
                    }
                }
                if (damageTimer <= 0)
                {
                    state = States.Cooldown;
                    damageTimer = damageTimerDefault;
                    spriteRenderer.sprite = cooldownSprite;
                }
                break;

            case States.Cooldown:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    state = States.Primed;
                    cooldownTimer = cooldownTimerDefault;
                    spriteRenderer.sprite = primedSprite;
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            players.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            players.Remove(other.gameObject);
        }
    }
}
