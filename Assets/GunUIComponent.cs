﻿using System.Collections;
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
    // The background image of the slider.
    [SerializeField]
    private Image sliderBackground;
    // The fill image of the slider.
    [SerializeField]
    private Image sliderFill;

	[SerializeField]
	private VariableWeight weightedVariableType;
    /// <summary>
    /// The weapon variable associated with this UI element.
    /// </summary>
    public VariableWeight GetWeightedVariableType()
	{
		return weightedVariableType;
	}

    public void SetSliderBackgroundColor(Color newColor)
    {
        sliderBackground.color = newColor;
    }

    public void SetSliderFillColor(Color newColor)
    {
        sliderFill.color = newColor;
    }

	/// <summary>
	/// Sets the slider to the given color.
	/// </summary>
	/// <param name="fillColor">The color to set the slider to.</param>
	public void SetSliderColor(Color fillColor)
	{
		SetSliderFillColor(fillColor);

		// Make slider background a darker shade of fill color.
        Color backgroundColor = new Color
        (
            fillColor.r * 0.25f,
            fillColor.g * 0.25f,
            fillColor.b * 0.25f
        );
		SetSliderBackgroundColor(backgroundColor);
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

    // Initialization.
    private void Start()
    {
        slider.maxValue = weightedVariableType.maxedWeight;
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
