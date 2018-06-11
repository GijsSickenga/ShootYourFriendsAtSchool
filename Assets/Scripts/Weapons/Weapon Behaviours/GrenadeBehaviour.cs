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
    protected override void OnValidate()
    {
        base.OnValidate();

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
        Debug.Log(Settings.variableName + " executed with value: " + Settings.LerpWeightInt());
        if (NextBehaviour != null)
        {
            NextBehaviour.Activate(startPosition, startRotation);
        }
    }

    public override void OnTriggered(Vector3 position, Collision2D col)
    {
        throw new System.NotImplementedException();
    }
}
