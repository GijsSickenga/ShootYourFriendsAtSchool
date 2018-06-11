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
        Debug.Log(Settings.variableName + " executed with value: " + Settings.LerpWeight());
        if (NextBehaviour != null)
        {
            NextBehaviour.Activate(startPosition, startRotation);
        }
    }

    public override void OnTriggered(Vector3 position, Vector3 direction, BehaviourProjectile projectile)
    {
        throw new System.NotImplementedException();
    }
}
