using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeBehaviourScript", menuName = "Behaviour Class Instance/GrenadeBehaviour")]
public class GrenadeBehaviour : WeaponBehaviour
{
    public GrenadeBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
        // Reduce damage of projectiles spawned from explosion.
        if (NextBehaviour != null)
        {
            int newDamage = NextBehaviour.Stats._projectileDamage;
            newDamage /= Settings.LerpWeightInt() / 2;
            NextBehaviour.Stats.SetProjectileSpeed(newDamage);
        }
    }

    [SerializeField]
    GameObject _spawnedProjectileType;
    // Overridden to return the bullet type spawned by a grenade when asked for
    // bullet type by another behaviour. This prevents bouncing behaviour from
    // instantiating grenades when it comes after grenade behaviour.
    public override GameObject ProjectileType { get { return _spawnedProjectileType; } }

    // On UI update.
    protected void OnValidate()
    {
        // Check if prefab set.
        if (_spawnedProjectileType != null)
        {
            // Check if prefab contains BehaviourProjectile script.
            BehaviourProjectile projectileScript = _spawnedProjectileType.GetComponent<BehaviourProjectile>();
            if (projectileScript == null)
            {
                // No BehaviourProjectile script found, so reset to null.
                _spawnedProjectileType = null;
            }
        }
    }

    public override void Activate(Vector3 startPosition, Quaternion startRotation)
    {
        // Instantiate grenade.
        GameObject projectile = Instantiate(Settings.projectileType, startPosition, startRotation);
        // Grab projectile script.
        GrenadeProjectile projectileScript = (GrenadeProjectile)projectile.GetComponent<BehaviourProjectile>();
        // Initialize grenade.
        projectileScript.Initialize(OnTriggered, PlayerID, Settings, Stats);
    }

    public override void OnTriggered(Vector3 position, Vector3 direction)
    {
        if (NextBehaviour != null)
        {
            float rotationPerProjectile = 360 / (float)Settings.LerpWeightInt();
            for (int i = 0; i < Settings.LerpWeightInt(); i++)
            {
                Vector3 newRotation = new Vector3(0, 0, rotationPerProjectile * i);
                NextBehaviour.Activate(position, Quaternion.Euler(newRotation));
            }
        }
    }
}
