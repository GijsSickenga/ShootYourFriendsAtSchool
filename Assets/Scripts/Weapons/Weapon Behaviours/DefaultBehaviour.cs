using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultBehaviourScript", menuName = "Behaviour Class Instance/DefaultBehaviour")]
public class DefaultBehaviour : WeaponBehaviour
{
    public DefaultBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Activate(Vector3 startPosition, Quaternion startRotation)
    {
        // Instantiate bullet type at given position with given rotation.
        GameObject projectile = Instantiate(ProjectileType, startPosition, startRotation);
        // Grab projectile script (and optionally cast to specific projectile type).
        DefaultProjectile projectileScript = (DefaultProjectile)projectile.GetComponent<BehaviourProjectile>();
        // Set delegate to this behaviour's trigger event.
        projectileScript.OnTriggerBehaviour = OnTriggered;
        
        // Do other stuff with the new projectile here (initialization, etc.).
    }

    public override void OnTriggered(Vector3 position, Collision2D col)
    {
        if (NextBehaviour != null)
        {
            // Do some calculations based on passed in collision data.
            // Calculate a position and rotation for the next behaviour to spawn at.
            Vector3 newPosition = position + new Vector3(1, 2, 3);
            Vector3 newRotation = new Vector3(1, 2, 3);

            // Activate the next behaviour at the calculated position.
            // Do this in a loop if you want to spawn multiple projectiles of the next behaviour.
            NextBehaviour.Activate(newPosition, Quaternion.Euler(newRotation));
        }
    }
}
