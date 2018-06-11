﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceBehaviourScript", menuName = "Behaviour Class Instance/BounceBehaviour")]
public class BounceBehaviour : WeaponBehaviour
{
    public BounceBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Activate(Vector3 startPosition, Quaternion startRotation)
    {
        if (PreviousBehaviour != null)
        {
            // Instantiate previous projectile.
            GameObject projectile = Instantiate(PreviousBehaviour.ProjectileType, startPosition, startRotation);
            // Grab projectile script.
            BehaviourProjectile projectileScript = projectile.GetComponent<BehaviourProjectile>();
            // Initialize previous projectile.
            projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);

            // Assign bounce script.
            BounceCounter counter = projectile.AddComponent<BounceCounter>();
            counter.numberOfBounces = Settings.LerpWeightInt();
        }
        else
        {
            // Instantiate default projectile.
            GameObject projectile = Instantiate(ProjectileType, startPosition, startRotation);
            // Grab projectile script.
            BehaviourProjectile projectileScript = projectile.GetComponent<BehaviourProjectile>();
            // Initialize default projectile.
            projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);

            // Assign bounce script.
            BounceCounter counter = projectile.AddComponent<BounceCounter>();
            counter.numberOfBounces = Settings.LerpWeightInt();
        }
    }

    public override void OnTriggered(Vector3 position, Vector3 direction, BehaviourProjectile projectile)
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
                // Initialize previous projectile.
                projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);

                // Assign bounce script.
                BounceCounter newCounter = newProjectile.AddComponent<BounceCounter>();
                counter.numberOfBounces = counter.numberOfBounces - 1;
            }
            else
            {
                // Instantiate default projectile.
                GameObject newProjectile = Instantiate(ProjectileType, position, Quaternion.Euler(direction));
                // Grab projectile script.
                BehaviourProjectile projectileScript = newProjectile.GetComponent<BehaviourProjectile>();
                // Initialize default projectile.
                projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);

                // Assign bounce script.
                BounceCounter newCounter = newProjectile.AddComponent<BounceCounter>();
                counter.numberOfBounces = counter.numberOfBounces - 1;
            }
        }
        else
        {
            if (NextBehaviour != null)
            {
                // Activate next behaviour, since bouncing is done.
                NextBehaviour.Activate(position, Quaternion.Euler(direction));
            }
        }
    }
}
