using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserBehaviourScript", menuName = "Behaviour Class Instance/LaserBehaviour")]
public class LaserBehaviour : WeaponBehaviour
{
    public LaserBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Execute(Vector3 startPosition, Quaternion startRotation)
    {
        Debug.Log(Settings.variableName + " executed with value: " + Settings.LerpWeight());
        if (NextBehaviour != null)
        {
            NextBehaviour.Execute(startPosition, startRotation);
        }
    }
}
