using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour
{
	public struct WeaponStats
	{
		float bulletSpeed;
		int bulletDamage;
		Color bulletColor;
	}

	public WeaponBehaviour(float weight, WeaponStats stats, WeaponBehaviour nextBehaviour)
	{
        _weight = weight;
		_stats = stats;
		_nextBehaviour = nextBehaviour;
	}

	protected float _weight;
	public float Weight
	{
		get
		{
			return _weight;
		}
	}

	protected WeaponStats _stats;
    public WeaponStats Stats
    {
        get
        {
            return _stats;
        }
    }

	protected WeaponBehaviour _nextBehaviour;

	/// <summary>
	/// Implement specific behaviour in subclass.
	/// </summary>
	public abstract void Execute(WeaponBehaviour previousBehaviour);
}
