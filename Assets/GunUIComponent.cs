using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GunUIComponent : MonoBehaviour
{
	[SerializeField]
    private Text weightText;
    /// <summary>
    /// The weight amount for the component.
    /// </summary>
    public Text GetWeightText()
	{
		return weightText;
	}

    // The slider that shows how much of the max weight is put into the component.
	[SerializeField]
    private Slider slider;

	[SerializeField]
	private VariableWeight weightedVariableType;
    /// <summary>
    /// The weapon variable associated with this UI element.
    /// </summary>
    public VariableWeight GetWeightedVariableType()
	{
		return weightedVariableType;
	}

	/// <summary>
	/// Sets the slider color to the color of the player corresponding to the given ID.
	/// </summary>
	/// <param name="playerID">The ID of the player to grab the color from.</param>
	public void SetSliderColor(int playerID)
	{
		//slider.colors.normalColor = ColorTracker.colors[playerID];
	}

	// Initialization.
	private void Start()
	{
		slider.maxValue = weightedVariableType.maxedWeight;
	}

	/// <summary>
	/// Sets the weight text parameter of the component.
	/// </summary>
	/// <param name="newWeight">The new weight to set the weight text to.</param>
    public void SetWeight(float newWeight)
    {
		// Set weight text and slider value.
        weightText.text = newWeight.ToString("0.0") + "/" + weightedVariableType.maxedWeight.ToString("0.0");
		slider.value = newWeight;
    }

	// Update UI elements.
	private void OnValidate()
	{
		// Set names to weighted variable name if it's set.
		if (weightedVariableType != null)
        {
            GetComponent<Text>().text = weightedVariableType.variableName + ":";
            name = weightedVariableType.variableName;
		}
	}
}
