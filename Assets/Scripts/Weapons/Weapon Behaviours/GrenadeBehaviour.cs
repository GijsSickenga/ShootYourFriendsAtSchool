using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeBehaviourScript", menuName = "Behaviour Class Instance/GrenadeBehaviour")]
public class GrenadeBehaviour : WeaponBehaviour
{
    public GrenadeBehaviour(int playerID, BehaviourWeight behaviourSettings, WeaponBehaviour nextBehaviour) : base(playerID, behaviourSettings, nextBehaviour)
    {
    }

    public override void Activate(Vector3 startPosition, Quaternion startRotation)
    {
        Debug.Log(Settings.variableName + " executed with value: " + Settings.LerpWeightInt());
        if (NextBehaviour != null)
        {
            NextBehaviour.Activate(startPosition, startRotation);
        }
    }
}
