using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultBehaviourScript", menuName = "Behaviour Class Instance/DefaultBehaviour")]
public class DefaultBehaviour : WeaponBehaviour
{
    public DefaultBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Activate(Vector3 startPosition, Quaternion startRotation, Collider2D col = null)
    {
        // Instantiate bullet type at given position with given rotation.
        GameObject projectile = Instantiate(ProjectileType, startPosition, startRotation);
        // Grab projectile script (and optionally cast to specific projectile type).
        DefaultProjectile projectileScript = (DefaultProjectile)projectile.GetComponent<BehaviourProjectile>();
        // Set delegate to this behaviour's trigger event.
        projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);
        
        // Do other stuff with the new projectile here (initialization, etc.).
    }

    public override void OnTriggered(Vector3 position, Vector3 direction, BehaviourProjectile projectile, Collider2D col = null)
    {
        if (NextBehaviour != null)
        {
            // Activate the next behaviour at the calculated position.
            // Do this in a loop if you want to spawn multiple projectiles of the next behaviour.
            NextBehaviour.Activate(position, Quaternion.Euler(direction), col);
        }
    }
}
