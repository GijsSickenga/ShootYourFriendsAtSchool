using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specifies a weapon behaviour.
/// Behaviours can be linked in a chain.
/// Each behaviour has references to the previous and next behaviour in the chain,
/// which are null if there is no behaviour before or after it.
/// </summary>
[System.Serializable]
public abstract class WeaponBehaviour : ScriptableObject
{
	/// <summary>
	/// A struct containing weapon stats relevant to behaviours.
	/// </summary>
	public struct WeaponStats
	{
		public WeaponStats(float projectileSpeed, int projectileDamage, Color projectileColor)
        {
            this.projectileSpeed = projectileSpeed;
            this.projectileDamage = projectileDamage;
            this.projectileColor = projectileColor;
		}

		public float projectileSpeed;
        public int projectileDamage;
        public Color projectileColor;
	}

    /// <summary>
    /// Initializes the behaviour.
	/// Call in recursive function to initialize a behaviour chain.
    /// </summary>
    /// <param name="playerID">ID of the player holding the weapon this behaviour will be attached to.</param>
    /// <param name="behaviourSettings">The settings for this.</param>
    /// <param name="nextBehaviour">The next behaviour in the behaviour chain.</param>
    public WeaponBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour)
	{
		_playerID = playerID;
        _behaviourSettings = behaviourSettings;
		_nextBehaviour = nextBehaviour;

		// Finish initialization by passing this behaviour to the next one in the chain.
		if (_nextBehaviour != null)
		{
			_nextBehaviour._previousBehaviour = this;
		}
    }

    private int _playerID;
    private BehaviourWeight _behaviourSettings;
    private WeaponBehaviour _previousBehaviour;
    private WeaponBehaviour _nextBehaviour;
    private WeaponStats _stats;
	
	/// <summary>
	/// The ID of the player that is holding the weapon this behaviour is attached to.
	/// </summary>
	public int PlayerID { get { return _playerID; } }

	/// <summary>
	/// The settings assigned to this behaviour.
	/// </summary>
	public BehaviourWeight Settings { get { return _behaviourSettings; } }

    /// <summary>
    /// The previous behaviour in the behaviour chain.
    /// </summary>
    public WeaponBehaviour PreviousBehaviour { get { return _previousBehaviour; } }

    /// <summary>
    /// The next behaviour in the behaviour chain.
    /// </summary>
    public WeaponBehaviour NextBehaviour { get { return _nextBehaviour; } }

    /// <summary>
    /// Contains relevant stats of the weapon this behaviour is attached to.
	/// Setting this property on the first behaviour propagates the stats
	/// through the behaviour chain.
    /// </summary>
    public WeaponStats Stats
    {
        get
        {
            return _stats;
        }
		
		set
		{
			_stats = value;
			if (_nextBehaviour != null)
            {
                // Propagate weapon stats through the behaviour chain.
                _nextBehaviour.Stats = value;
			}
		}
    }

    [SerializeField]
    private GameObject _projectileType;
	/// <summary>
	/// The projectile type prefab this behaviour spawns in Activate().
	/// </summary>
	public virtual GameObject ProjectileType { get { return _projectileType; } }

	// On UI update.
    protected virtual void OnValidate()
    {
        // Check if prefab set.
        if (_projectileType != null)
        {
            // Check if prefab contains BehaviourProjectile script.
            BehaviourProjectile projectileScript = _projectileType.GetComponent<BehaviourProjectile>();
            if (projectileScript == null)
            {
                // No BehaviourProjectile script found, so reset to null.
                _projectileType = null;
            }
        }
    }

	/// <summary>
	/// Should be called by previous behaviour (or player weapon).
	/// Should spawn an instance of the projectile type set in the behaviour.
	/// </summary>
	public abstract void Activate(Vector3 startPosition, Quaternion startRotation);

	/// <summary>
	/// Should be called by projectiles when they want to trigger the next behaviour.
	/// Should call Activate() on the next behaviour for all positions where the current
	/// behaviour wants to create a new projectile.
	/// </summary>
	public abstract void OnTriggered(Vector3 position, Collision2D col);
}
