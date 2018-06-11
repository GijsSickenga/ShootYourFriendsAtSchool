using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Weighted Grenade Behaviour", menuName = "Weighted Grenade Behaviour")]
public class GrenadeWeight : BehaviourWeight
{
    [Tooltip("The projectile type associated with this behaviour.")]
    public GameObject spawnedProjectileType;
}
