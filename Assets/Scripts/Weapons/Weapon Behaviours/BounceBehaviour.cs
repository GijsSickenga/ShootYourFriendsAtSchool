using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceBehaviourScript", menuName = "Behaviour Class Instance/BounceBehaviour")]
public class BounceBehaviour : WeaponBehaviour
{
    public BounceBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Activate(Vector3 startPosition, Quaternion startRotation, Collider2D col = null)
    {
        if (PreviousBehaviour != null)
        {
            // Instantiate previous projectile.
            GameObject projectile = Instantiate(PreviousBehaviour.ProjectileType, startPosition, startRotation);
            // Grab projectile script.
            BehaviourProjectile projectileScript = projectile.GetComponent<BehaviourProjectile>();

            // Assign bounce script.
            BounceCounter counter = projectile.AddComponent<BounceCounter>();
            counter.numberOfBounces = Settings.LerpWeightInt();

            // Initialize previous projectile.
            projectileScript.Initialize(OnTriggered, PlayerID, PreviousBehaviour.Settings, Stats);
        }
        else
        {
            // Instantiate default projectile.
            GameObject projectile = Instantiate(ProjectileType, startPosition, startRotation);
            // Grab projectile script.
            BehaviourProjectile projectileScript = projectile.GetComponent<BehaviourProjectile>();

            // Assign bounce script.
            BounceCounter counter = projectile.AddComponent<BounceCounter>();
            counter.numberOfBounces = Settings.LerpWeightInt();

            // Initialize default projectile.
            projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);
        }
    }

    public override void OnTriggered(Vector3 position, Vector3 direction, BehaviourProjectile projectile, Collider2D col = null)
    {
        // Grab bounce script.
        BounceCounter counter = projectile.GetComponent<BounceCounter>();
        if (counter.numberOfBounces > 0)
        {
            if (PreviousBehaviour != null)
            {
                // Instantiate previous projectile.
                GameObject newProjectile = Instantiate(PreviousBehaviour.ProjectileType, position, Quaternion.Euler(direction));
                // Grab projectile script.
                BehaviourProjectile projectileScript = newProjectile.GetComponent<BehaviourProjectile>();

                // Assign bounce script.
                BounceCounter newCounter = newProjectile.AddComponent<BounceCounter>();
                newCounter.numberOfBounces = counter.numberOfBounces - 1;

                // Initialize previous projectile.
                projectileScript.Initialize(OnTriggered, PlayerID, PreviousBehaviour.Settings, Stats);

                if (col != null)
                    Physics2D.IgnoreCollision(newProjectile.GetComponent<Collider2D>(), col.GetComponent<Collider2D>());
            }
            else
            {
                // Instantiate default projectile.
                GameObject newProjectile = Instantiate(ProjectileType, position, Quaternion.Euler(direction));
                // Grab projectile script.
                BehaviourProjectile projectileScript = newProjectile.GetComponent<BehaviourProjectile>();

                // Assign bounce script.
                BounceCounter newCounter = newProjectile.AddComponent<BounceCounter>();
                newCounter.numberOfBounces = counter.numberOfBounces - 1;
                
                // Initialize default projectile.
                projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);

                if (col != null)
                    Physics2D.IgnoreCollision(newProjectile.GetComponent<Collider2D>(), col.GetComponent<Collider2D>());
            }
        }
        else
        {
            if (NextBehaviour != null)
            {
                // Activate next behaviour, since bouncing is done.
                NextBehaviour.Activate(position, Quaternion.Euler(direction), col);
            }
        }
    }
}
