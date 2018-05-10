using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallingPlatform : MonoBehaviour
{
    public enum States { Up, Warning, Falling, Down, Holding, Rising };
    private States state = States.Up;
    public States GetState
    {
        get
        {
            return state;
        }
    }

    public float warningTimer = 2.0f;
    public Vector2 warningTimerRandomDelay;
    private float warningTimerDefault;

    public float fallingTimer = 2.0f;
    private float fallingTimerDefault;

    public float holdingTimer = 2.0f;
    public Vector2 holdingTimerRandomDelay;
    private float holdingTimerDefault;

    public float risingTimer = 1.5f;
    private float risingTimerDefault;

    private Transform sprite;
    private Vector3 spriteScale;
    private Vector3 spriteDefaultPosition;
    private Color upColor, downColor;

    // TODO: Implement a list of child objects to change color for every object on top of this platform when it drops

    // List of colliders that are disabled when the platform drops - used to reenable them when it has risen back up
    private List<Collider2D> disabledColliders = new List<Collider2D>();

    void Start()
    {
        // Initialize reset values for timers
        warningTimerDefault = warningTimer;
        fallingTimerDefault = fallingTimer;
        holdingTimerDefault = holdingTimer;
        risingTimerDefault = risingTimer;

        // Initialize reset values for sprite properties
        sprite = transform.GetChild(0);
        spriteScale = sprite.localScale;
        spriteDefaultPosition = sprite.position;
        upColor = sprite.GetComponent<SpriteRenderer>().color;
        downColor = new Color(0, 0, upColor.b + 0.4f);
    }

    void Update()
    {
        switch (state)
        {
            case States.Up:
                // Waiting state - call Drop() to break out of this state
                break;

            case States.Warning:
                warningTimer -= Time.deltaTime;
                // TODO: Implement a better warning animation here.
                //       Right now the platform just wiggles back and forth a bit.
                sprite.position = spriteDefaultPosition + new Vector3(Random.Range(-0.04f, 0.04f),
                                                                      Random.Range(-0.04f, 0.04f),
                                                                      0);
                if (warningTimer <= 0)
                {
                    state = States.Falling;
                    // Reset the sprite's position so it's in the right place and not offset by the warning animation
                    sprite.position = spriteDefaultPosition;
                    // Get the size of the sprite so we know what value to start easing from in the next state
                    spriteScale = sprite.localScale;
                    
                    // Disable all colliders in children to stop unwanted behaviour after objects are dropped
                    foreach (Collider2D currentCollider in GetComponentsInChildren<Collider2D>())
                    {
                        if (currentCollider.enabled)// && currentCollider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                        {
                            currentCollider.enabled = false;
                            disabledColliders.Add(currentCollider);
                        }
                    }
                }
                break;
            
            case States.Falling:
                fallingTimer -= Time.deltaTime;

                float fallingEaseTimer = fallingTimerDefault - fallingTimer;
                if (fallingEaseTimer < fallingTimerDefault)
                {
                    // Ease scale to 0 to make platform disappear
                    sprite.localScale = Easing.FallingPlatformDown(fallingEaseTimer, spriteScale, new Vector3(0, 0, spriteScale.z), fallingTimerDefault);

                    // Fade color to dark blue
                    //sprite.GetComponent<SpriteRenderer>().color = Easing.FallingPlatformDown(fallingEaseTimer, upColor, downColor, fallingTimerDefault);
                }
                else
                {
                    // Set scale to 0 so it doesn't go into negative
                    sprite.localScale = new Vector3(0, 0, spriteScale.z);
                    // Set color to end color
                    //sprite.GetComponent<SpriteRenderer>().color = downColor;
                }

                if (fallingTimer <= 0)
                {
                    state = States.Down;
                    fallingTimer = fallingTimerDefault;
                    // Turn off sprite while the platform is down
                    sprite.gameObject.SetActive(false);
                }
                break;

            case States.Down:
                // Waiting state - call Rise() to break out of this state
                break;

            case States.Holding:
                holdingTimer -= Time.deltaTime;

                if (holdingTimer <= 0)
                {
                    state = States.Rising;
                    // Reenable sprite before platform starts rising
                    sprite.gameObject.SetActive(true);
                }
                break;

            case States.Rising:
                risingTimer -= Time.deltaTime;

                float risingEaseTimer = risingTimerDefault - risingTimer;
                if (risingEaseTimer < risingTimerDefault)
                {
                    // Ease scale back to normal to make platform rise
                    sprite.localScale = Easing.InQuintic(risingEaseTimer, new Vector3(0, 0, spriteScale.z), spriteScale, risingTimerDefault);

                    // Fade color back to normal
                    //sprite.GetComponent<SpriteRenderer>().color = Easing.InQuintic(risingEaseTimer, downColor, upColor, risingTimerDefault);
                }
                else
                {
                    // Reset scale to normal
                    sprite.localScale = spriteScale;
                    // Reset color to normal
                    //sprite.GetComponent<SpriteRenderer>().color = upColor;
                }

                if (risingTimer <= 0)
                {
                    state = States.Up;
                    risingTimer = risingTimerDefault;

                    // Reenable all colliders in children when platform has risen
                    foreach (Collider2D currentCollider in disabledColliders)
                    {
                        currentCollider.enabled = true;
                    }

                    // Empty list so it can be reused next cycle
                    disabledColliders.Clear();
                }
                break;
        }
    }

    public void Drop()
    {
        if (state == States.Up)
        {
            state = States.Warning;
            warningTimer = warningTimerDefault + Random.Range(warningTimerRandomDelay.x, warningTimerRandomDelay.y);
        }
    }
    public void Drop(float minDelay, float maxDelay)
    {
        if (state == States.Up)
        {
            state = States.Warning;
            warningTimer = warningTimerDefault + Random.Range(minDelay, maxDelay);
        }
    }

    public void Rise()
    {
        if (state == States.Down)
        {
            state = States.Holding;
            holdingTimer = holdingTimerDefault + Random.Range(holdingTimerRandomDelay.x, holdingTimerRandomDelay.y);
        }
    }
    public void Rise(float minDelay, float maxDelay)
    {
        if (state == States.Down)
        {
            state = States.Holding;
            holdingTimer = holdingTimerDefault + Random.Range(minDelay, maxDelay);
        }
    }
}
