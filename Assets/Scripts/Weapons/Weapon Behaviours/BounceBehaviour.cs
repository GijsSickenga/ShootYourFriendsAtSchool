using System.Collections;
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
