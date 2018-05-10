using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon Variable", menuName = "Weapon Variable")]
public class VariableWeight : ScriptableObject
{
	public string variableName;
	
	[HideInInspector]
	public float currentValue;

	public float worstValue;
	public float bestValue;

	public float maxedWeight;

	public bool isIntegerValue;

	public float LerpWeight()
	{
		// Lerp between the possible values based on the weight opposed to the max weight.
		return Mathf.Lerp(worstValue, bestValue, currentValue / maxedWeight);
	}

	public int LerpWeightInt()
	{
		return Mathf.RoundToInt(LerpWeight());
	}
}
