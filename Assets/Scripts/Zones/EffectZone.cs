using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/********************************** TODO ***********************************

    * Integrate particle systems into the effect zone
        - These should show a different effect based on the ZoneType
        - Implement particles for:
            + entering/leaving the zone
            + activating a discrete/overtime UpdateValue() call
            + different visual when easing?
    * Check for actual collision with player before updating stats
    * Implement functionality for other game objects (specifically projectiles)
    * Make all timers player-dependant so each player's timer only runs down when they are inside the zone
    * Work on all the TODO's in this file :p
    
************************************ TODO *********************************/

public class EffectZone : MonoBehaviour
{
    //
    // Summary:
    //     ///
    //     Types of zones that can be chosen from.
    //     The type of zone will determine which attribute is affected by the zone.
    //     ///
    //
    // ZoneTypes:
    //   Damage:
    //     This will decrease the target's health.
    //     Visual effect is red.
    //
    //   Heal:
    //     This will increase the target's health.
    //     Visual effect is green.
    //
    //   SlowDownTime:
    //     This will slow the target's relative passage of time.
    //     Visual effect is purple.
    //
    //   SpeedUpTime:
    //     This will speed up the target's relative passage of time.
    //     Visual effect is yellow.
    //
    public enum ZoneType
    {
        Damage, Heal,
        SlowDownTime, SpeedUpTime
    };
    public ZoneType zoneType;
    
    //
    // Summary:
    //     ///
    //     Types of timescales that determine the zone's effects over time.
    //     ///
    //
    // TimeScales:
    //   Discrete:
    //     Increases a value by a set amount once and keeps it there.
    //     Useful for things like invulnerability or time distortions.
    //     Easing will increase the value over time until it reaches its target.
    //
    //   OverTime:
    //     Continuously increases a value by a set amount for an indefinite time period.
    //     Easing will increase the value over time until it reaches its target, after which
    //     the next cycle will start.
    //
    public enum TimeScale { Discrete, OverTime };
    public TimeScale timeScale;

    public enum UpdateType { Flat, Multiplier, PercentageOfMax };
    public UpdateType updateType;

    //
    // Summary:
    //     ///
    //     The amount to increase the zone's value by.
    //     This value is determined by the ZoneType.
    //     If easing is used, the amount will be reached over a set time period,
    //     rather than instantly.
    //     ///
    //
    public float amount;

    //
    // Summary:
    //     ///
    //     Easing will increase a value by amount over easeTime seconds,
    //     rather than instantly.
    //     0 = no easing
    //     ///
    //
    private bool easing = false;
    public float easeTime;
    private float easeTimer;

    //
    // Summary:
    //     ///
    //     Determines the interval between value increases.
    //     Only used in OverTime timescale.
    //     ///
    //
    public Vector2 intervalTimeRange;
    private float intervalTimer;

    private bool hasUpdated = false;
    private bool updatingValue = false;
    
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        easeTimer = easeTime;
        intervalTimer = Random.Range(intervalTimeRange.x, intervalTimeRange.y);

        if (easeTime > 0)
        {
            easing = true;
        }
    }

    void Update()
    {
        if (timeScale == TimeScale.Discrete)
        {
            if (!hasUpdated)
            {
                UpdateValueOverTime(amount);
                // TODO: Make it so this value is tracked for each player as long as they are in the zone.
                //       As long as the player is in the zone they cannot receive another stat update.
                hasUpdated = true;
            }
        }
        else if (timeScale == TimeScale.OverTime)
        {
            if (updatingValue)
            {
                if (easing)
                {
                    // Check if the easeTimer is about to run out
                    if ((easeTimer - Time.deltaTime) <= 0)
                    {
                        // Add the remainder
                        UpdateValueOverTime(amount / easeTime * easeTimer);
                        // Reset
                        easeTimer = easeTime;
                        updatingValue = false;
                    }
                    else
                    {
                        UpdateValueOverTime(amount / easeTime * Time.deltaTime);
                        easeTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    UpdateValueOverTime(amount);
                    updatingValue = false;
                }
            }
            else
            {
                if (intervalTimer > 0)
                {
                    intervalTimer -= Time.deltaTime;
                }
                else
                {
                    updatingValue = true;
                    intervalTimer = Random.Range(intervalTimeRange.x, intervalTimeRange.y);
                }
            }
        }
    }

    void UpdateValueOverTime(float amount)
    {
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                Health playerHealth = player.GetComponent<Health>();

                switch (zoneType)
                {
                    case ZoneType.Damage:
                        switch (updateType)
                        {
                            case UpdateType.Flat: playerHealth.Damage(amount); break;
                            case UpdateType.Multiplier: playerHealth.Damage(amount * playerHealth.currentHealth); break;
                            case UpdateType.PercentageOfMax: playerHealth.Damage((amount / 100) * playerHealth.maxHealth); break;
                        }
                        break;

                    case ZoneType.Heal:
                        switch (updateType)
                        {
                            case UpdateType.Flat: playerHealth.Heal(amount); break;
                            case UpdateType.Multiplier: playerHealth.Heal(amount * playerHealth.currentHealth); break;
                            case UpdateType.PercentageOfMax: playerHealth.Heal((amount / 100) * playerHealth.maxHealth); break;
                        }
                        break;

                    case ZoneType.SlowDownTime:
                        // TODO: Implement time variable in player so it can be changed
                        break;
                    
                    case ZoneType.SpeedUpTime:
                        // TODO: Implement time variable in player so it can be changed
                        break;
                }
            }
            else if (amount <= 0)
            {
                Debug.Log("\"amount\" needs to be higher than 0, otherwise this zone [" + name + ", " + GetInstanceID() + "] will not work.");
            }
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
