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

        // Stats.projectileDamage
        // Stats.projectileColor
        // Settings.LerpWeight();
        // PlayerID
    }

    public override void OnTriggered(Vector3 position, Collision2D col)
    {
        throw new System.NotImplementedException();
    }
}
