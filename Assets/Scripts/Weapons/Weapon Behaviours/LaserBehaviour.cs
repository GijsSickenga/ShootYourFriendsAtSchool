using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserBehaviourScript", menuName = "Behaviour Class Instance/LaserBehaviour")]
public class LaserBehaviour : WeaponBehaviour
{
    public LaserBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Activate(Vector3 startPosition, Quaternion startRotation)
    {
        // Instantiate bullet type at given position with given rotation.
        GameObject projectile = Instantiate(ProjectileType, startPosition, startRotation);
        // Grab projectile script (and optionally cast to specific projectile type).
        LaserProjectile projectileScript = (LaserProjectile)projectile.GetComponent<BehaviourProjectile>();
        // Set delegate to this behaviour's trigger event.
        projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);
    }

    public override void OnTriggered(Vector3 position, Vector3 direction)
    {
        if(NextBehaviour != null)
        {
            NextBehaviour.Activate(position, Quaternion.Euler(direction));
        }
    }
}
