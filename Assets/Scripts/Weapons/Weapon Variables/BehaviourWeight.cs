using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Weighted Behaviour", menuName = "Weighted Behaviour")]
public class BehaviourWeight : VariableWeight
{
	/// <summary>
	/// Whether the behaviour has been enqueued yet.
	/// </summary>
	[HideInInspector]
	public bool enqueued;

	[Tooltip("The weight required to activate this behaviour.")]
	public float thresholdWeight;

    [Tooltip("The behaviour script associated with this behaviour.")]
	public WeaponBehaviour behaviourScript;

    [Tooltip("The projectile type associated with this behaviour.")]
    public GameObject projectileType;

    // On UI update.
    public override void OnValidate()
    {
		base.OnValidate();

        // Check if prefab set.
        if (projectileType != null)
        {
            // Check if prefab contains BehaviourProjectile script.
            BehaviourProjectile projectileScript = projectileType.GetComponent<BehaviourProjectile>();
            if (projectileScript == null)
            {
                // No BehaviourProjectile script found, so reset to null.
                projectileType = null;
            }
        }
    }

	public override float LerpWeight()
	{
		// Return 0 if below threshold weight.
		if (currentValue < thresholdWeight)
		{
			return 0;
		}
		else
        {
            // Lerp between the possible values based on the weight opposed to the
            // max weight minus the threshold weight, so the range of worst to best
			// value is distributed over the range of threshold to maxed weight.
            return Mathf.Lerp(worstValue, bestValue, (currentValue - thresholdWeight) / (maxedWeight - thresholdWeight));
		}
	}
}
