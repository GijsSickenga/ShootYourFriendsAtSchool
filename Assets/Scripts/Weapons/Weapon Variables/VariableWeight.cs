using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Weighted Variable", menuName = "Weighted Variable")]
public class VariableWeight : ScriptableObject
{
	public string variableName;

    public bool isIntegerValue;

	// Minimum range in OnValidate
	public int bias = 1;
	
	[HideInInspector]
	public float currentValue;

	public float worstValue;
	public float bestValue;

	public float maxedWeight;

	public virtual float LerpWeight()
	{
		// Lerp between the possible values based on the weight opposed to the max weight.
		return Mathf.Lerp(worstValue, bestValue, currentValue / maxedWeight);
	}

	public int LerpWeightInt()
	{
		return Mathf.RoundToInt(LerpWeight());
	}

	public virtual void OnValidate()
	{
		bias = Mathf.Max(bias, 0);
	}
}
