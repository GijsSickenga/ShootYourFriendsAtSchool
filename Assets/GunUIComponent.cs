using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GunUIComponent : MonoBehaviour
{
    // The weight amount for the component.
	[SerializeField]
    private Text weightText;

    // The slider that shows how much of the max weight is put into the component.
	[SerializeField]
    private Slider slider;

	[SerializeField]
	private VariableWeight weaponVariableType;
    /// <summary>
    /// The weapon variable associated with this UI element.
    /// </summary>
    public VariableWeight GetWeaponVariableType()
	{
		return weaponVariableType;
	}

	// Initialization.
	private void Start()
	{
		slider.maxValue = weaponVariableType.maxedWeight;
	}

	/// <summary>
	/// Sets the weight text parameter of the component.
	/// </summary>
	/// <param name="newWeight">The new weight to set the weight text to.</param>
    public void SetWeight(float newWeight)
    {
		// Set weight text and slider value.
        weightText.text = newWeight.ToString("0.0") + "/" + weaponVariableType.maxedWeight.ToString("0.0");
		slider.value = newWeight;
    }
}
