using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lever : MonoBehaviour
{
    public PitfallRoomManager prm;
    public float activeTimer = 1f;
    private float activeTimerDefault;
    private bool activated = false;

    private List<GameObject> players = new List<GameObject>();
    private SpriteRenderer sprite;

    public Sprite offSprite, onSprite;

    void Start()
    {
        activeTimerDefault = activeTimer;
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = offSprite;
    }

    void Update()
    {
        if (activated)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0)
            {
                activated = false;
                sprite.sprite = offSprite;
            }
        }
        else
        {
            foreach (GameObject player in players)
            {
                if (player.GetComponent<LocalPlayerController>().isInteracting)
                {
                    Activate();
                }
            }
        }
    }

    private void Activate()
    {
        prm.Activate();
        activeTimer = activeTimerDefault;
        activated = true;
        sprite.sprite = onSprite;
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
