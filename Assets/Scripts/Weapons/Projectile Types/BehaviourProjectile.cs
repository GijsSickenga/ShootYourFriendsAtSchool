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
}
