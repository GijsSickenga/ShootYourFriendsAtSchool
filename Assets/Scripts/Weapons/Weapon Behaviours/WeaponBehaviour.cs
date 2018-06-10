using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour : ScriptableObject
{
	public struct WeaponStats
	{
		public WeaponStats(float projectileSpeed, int projectileDamage, Color projectileColor)
        {
            this.projectileSpeed = projectileSpeed;
            this.projectileDamage = projectileDamage;
            this.projectileColor = projectileColor;
		}

		float projectileSpeed;
		int projectileDamage;
		Color projectileColor;
	}

	public WeaponBehaviour(int playerID, float weight, WeaponBehaviour previousBehaviour, WeaponBehaviour nextBehaviour)
	{
		_playerID = playerID;
        _weight = weight;
        _previousBehaviour = previousBehaviour;
		_nextBehaviour = nextBehaviour;
	}

	private int _playerID;
	public int PlayerID
	{
		get
		{
			return _playerID;
		}
	}

	private float _weight;
	public float Weight
	{
		get
		{
			return _weight;
		}
	}

	private WeaponStats _stats;
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

	// Surrounding behaviours in the behaviour chain.
    private WeaponBehaviour _previousBehaviour;
	public WeaponBehaviour PreviousBehaviour
	{
		get
		{
			return _previousBehaviour;
		}
	}
	private WeaponBehaviour _nextBehaviour;
    public WeaponBehaviour NextBehaviour
    {
        get
        {
            return _nextBehaviour;
        }
    }

	/// <summary>
	/// Implement specific behaviour in subclass.
	/// </summary>
	public abstract void Execute(Vector3 startPosition, Quaternion startRotation);
}
