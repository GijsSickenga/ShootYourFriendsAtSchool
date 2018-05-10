using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitfallRoom : MonoBehaviour
{
    public enum States { Up, Drop, Down, Rise };
    private States state = States.Up;
    public States GetState
    {
        get
        {
            return state;
        }
    }

    private List<FallingPlatform> platforms = new List<FallingPlatform>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            platforms.Add(child.GetComponent<FallingPlatform>());
        }
    }

    public void FixedUpdate()
    {
        switch (state)
        {
            case States.Up:
                // Waiting state - call Drop() to break out of this state
                break;

            case States.Drop:
                bool allPlatformsDown = true;
                foreach (FallingPlatform platform in platforms)
                {
                    if (platform.GetState != FallingPlatform.States.Down)
                    {
                        allPlatformsDown = false;
                        break;
                    }
                }

                if (allPlatformsDown)
                {
                    state = States.Down;
                }
                break;

            case States.Down:
                // Waiting state - call Rise() to break out of this state
                Rise();
                break;

            case States.Rise:
                bool allPlatformsUp = true;
                foreach (FallingPlatform platform in platforms)
                {
                    if (platform.GetState != FallingPlatform.States.Up)
                    {
                        allPlatformsUp = false;
                        break;
                    }
                }

                if (allPlatformsUp)
                {
                    state = States.Up;
                }
                break;
        }
    }

    public void Drop()
    {
        if (state == States.Up)
        {
            state = States.Drop;

            // Drop all platforms
            foreach (FallingPlatform platform in platforms)
            {
                platform.Drop();
            }
        }
    }

    public void Rise()
    {
        if (state == States.Down)
        {
            state = States.Rise;

            // Make all platforms rise
            foreach (FallingPlatform platform in platforms)
            {
                platform.Rise();
            }
        }
    }
}
