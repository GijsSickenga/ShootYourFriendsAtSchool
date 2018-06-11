using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a projectile, spawned by a behaviour.
/// Call OnTriggerBehaviour to inform the behaviour this was spawned by to call the next behaviour.
/// </summary>
public abstract class BehaviourProjectile : MonoBehaviour
{
	// Behaviour trigger callback delegate type.
	public delegate void BehaviourTrigger(Vector3 position, Collision2D col);
	/// <summary>
	/// The function to call when this projectile wants to call the next behaviour.
	/// Can be called on wall hit, explode, etc.
	/// Needs to pass along the arguments needed for the behaviour to handle the event.
	/// </summary>
    public BehaviourTrigger OnTriggerBehaviour;

	private int _playerIndex;
	public int PlayerID
    {
        get
        {
            return _playerIndex;
        }

        set
        {
            _playerIndex = value;
        }
	}

    private BehaviourWeight _behaviourWeight;
    /// <summary>
    /// Contains the settings of the corresponding behaviour.
    /// You can choose to use these in the bullet if you need.
    /// </summary>
    public BehaviourWeight BehaviourSettings
    {
        get
        {
            return _behaviourWeight;
        }

        set
        {
            _behaviourWeight = value;
        }
    }

    private WeaponBehaviour.WeaponStats _stats;
    /// <summary>
    /// Contains the stats of the corresponding weapon.
    /// </summary>
    public WeaponBehaviour.WeaponStats Stats
    {
        get
        {
            return _stats;
        }

        set
        {
            _stats = value;
        }
    }

	/// <summary>
	/// Should be called to initialize the projectile.
	/// </summary>
	public virtual void Initialize(BehaviourTrigger OnTriggerCallback, int playerID, BehaviourWeight settings, WeaponBehaviour.WeaponStats stats)
	{
        OnTriggerBehaviour = OnTriggerCallback;
        PlayerID = playerID;
        BehaviourSettings = settings;
		Stats = stats;
	}
}
