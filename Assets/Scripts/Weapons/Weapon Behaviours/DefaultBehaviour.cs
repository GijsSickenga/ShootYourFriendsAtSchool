using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultBehaviourScript", menuName = "Behaviour Class Instance/DefaultBehaviour")]
public class DefaultBehaviour : WeaponBehaviour
{
    public DefaultBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Execute(Vector3 startPosition, Quaternion startRotation)
    {
        Debug.Log(Settings.variableName + " executed...");
        if (NextBehaviour != null)
        {
            NextBehaviour.Execute(startPosition, startRotation);
        }
    }
}
