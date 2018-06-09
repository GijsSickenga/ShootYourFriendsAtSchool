using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
	public float startingWeaponWeight = 0;
	public float currentBaseWeaponWeight;
	public float weaponWeightIncrease = 5;

	public float refreshWeaponTime = 30;
	private float refreshWeaponTimer;

    // Lists of behaviours and variables.
	// These are set in the inspector and determine which behaviours and variables
	// can be assigned to the player weapons.
    public List<VariableWeight> weaponBehaviours = new List<VariableWeight>();
	public List<VariableWeight> weaponVariables = new List<VariableWeight>();

	// Gun UI parent objects, which contain references to specific UI elements,
	// in order to adjust gun stats per player.
	// These have to be in order (0-4) for the gun stats to correspond to the right players.
	public List<GunUIParent> gunUIParents = new List<GunUIParent>();

    // Dictionaries of behaviours and variables.
    private Dictionary<string, VariableWeight> weaponBehaviourDict = new Dictionary<string, VariableWeight>();
    private Dictionary<string, VariableWeight> weaponVariableDict = new Dictionary<string, VariableWeight>();

    /// <summary>
    /// Whether the initial generation has happened yet.
    /// </summary>
    private bool hasGeneratedFirst = false;

	// Get a reference to scalar, so it only needs to be found once
	private WeaponWeightScaler weaponWeightScalarRef = null;
	private WeaponWeightScaler WeaponWeightScaler
	{
		get
		{
			if(weaponWeightScalarRef == null)
				weaponWeightScalarRef = FindObjectOfType<WeaponWeightScaler>();

			return weaponWeightScalarRef;
		}
	}

    private void Start()
    {
		currentBaseWeaponWeight = startingWeaponWeight;

        // Initialize dictionaries from lists.
        foreach (VariableWeight var in weaponBehaviours)
        {
            weaponBehaviourDict.Add(var.variableName, var);
        }

        foreach (VariableWeight var in weaponVariables)
        {
            weaponVariableDict.Add(var.variableName, var);
        }

		ResetWeaponTimer();
	}
	
	private void Update()
	{
		refreshWeaponTimer -= Time.deltaTime;

		if(refreshWeaponTimer <= 0)
		{
			if(WeaponWeightScaler != null)
				WeaponWeightScaler.CalculateWeights();
            NewWeaponGeneration();
			ResetWeaponTimer();
		}

		if(Input.GetKeyDown(KeyCode.G))
		{
			// Debug force generate weapons
			if(WeaponWeightScaler != null)
				WeaponWeightScaler.CalculateWeights();
            NewWeaponGeneration();
			ResetWeaponTimer();
		}
	}

	/// <summary>
	/// Generates a new set of behaviours and weapon variables for all players.
	/// </summary>
	public void NewWeaponGeneration()
	{
		GenerateNewBehaviours();
		GenerateNewWeapons();
        currentBaseWeaponWeight += weaponWeightIncrease;
	}

	public void GenerateNewBehaviours()
	{
        Debug.Log("New behaviours generated with weight of: " + currentBaseWeaponWeight);
        foreach (LocalPlayerController player in GameData.players)
        {
            if (player != null)
            {
                // Player selected
                ResetValues(weaponBehaviourDict);

                // Generate weapon values
                CalculateWeaponBehaviours(player);

                // Grab a reference to this player's gun stats UI.
                GunUIParent gunUI = gunUIParents[player.playerIndex];
                // Adjust all gun stat values in UI.
                foreach (VariableWeight var in weaponVariableDict.Values)
                {
                    // Grab the component corresponding to the current type.
                    GunUIComponent component;
                    if (gunUI.components.TryGetValue(var.variableName, out component))
                    {
                        // UI component found, so write the new weight on the UI element.
                        component.SetWeight(var.currentValue);
                    }
                }

                // Link values to new weapon
                WeaponBase newWeapon = player.GiveDefaultWeapon();
                newWeapon.fireRate = weaponVariableDict["FireRate"].LerpWeight();
                newWeapon.bulletDeviation = weaponVariableDict["Deviation"].LerpWeight();
                newWeapon.bulletAmount = weaponVariableDict["BulletsPerShot"].LerpWeightInt();

                newWeapon.bulletSpeed = weaponVariableDict["BulletSpeed"].LerpWeight();
                newWeapon.bulletDamage = weaponVariableDict["Damage"].LerpWeightInt();
                newWeapon.bulletColor = ColorTracker.colors[player.playerIndex];

                newWeapon.ammoAmount = 0;
                newWeapon.spread = 30;
            }
        }

        currentBaseWeaponWeight += weaponWeightIncrease;
	}

    public void GenerateNewWeapons()
    {
		Debug.Log("New weapons generated with weight of: " + currentBaseWeaponWeight);
        foreach(LocalPlayerController player in GameData.players)
		{
			if (player != null)
			{
				// Player selected
				ResetValues(weaponVariableDict);

				// Generate weapon values
				CalculateWeaponVariables(player);

				// Grab a reference to this player's gun stats UI.
                GunUIParent gunUI = gunUIParents[player.playerIndex];
				// Adjust all gun stat values in UI.
				foreach(VariableWeight var in weaponVariableDict.Values)
				{
					// Grab the component corresponding to the current type.
					GunUIComponent component;
					if (gunUI.components.TryGetValue(var.variableName, out component))
					{
						// UI component found, so write the new weight on the UI element.
                        component.SetWeight(var.currentValue);
					}
				}

				// Link values to new weapon
				WeaponBase newWeapon = player.GiveDefaultWeapon();
				newWeapon.fireRate = weaponVariableDict["FireRate"].LerpWeight();
				newWeapon.bulletDeviation = weaponVariableDict["Deviation"].LerpWeight();
				newWeapon.bulletAmount = weaponVariableDict["BulletsPerShot"].LerpWeightInt();

				newWeapon.bulletSpeed = weaponVariableDict["BulletSpeed"].LerpWeight();
				newWeapon.bulletDamage = weaponVariableDict["Damage"].LerpWeightInt();
				newWeapon.bulletColor = ColorTracker.colors[player.playerIndex];

				newWeapon.ammoAmount = 0;
				newWeapon.spread = 30;
			}
		}

		currentBaseWeaponWeight += weaponWeightIncrease;
    }

	public void FirstGenerate()
	{
		if(!hasGeneratedFirst)
        {
            GenerateNewBehaviours();
			GenerateNewWeapons();
			hasGeneratedFirst = true;
		}
	}

	private void ResetValues(Dictionary<string, VariableWeight> variableWeights)
	{
		foreach(KeyValuePair<string, VariableWeight> pair in variableWeights)
		{
			pair.Value.currentValue = 0;
		}
    }

    private void CalculateWeaponBehaviours(LocalPlayerController player)
    {
        // Total weight to distribute at the start of generating behaviours
        float totalWeight = currentBaseWeaponWeight * player.weaponWeightScalar;

        Debug.Log("Player #" + player.playerIndex + " - Behaviours Weight: " + totalWeight);

        // List of remaining behaviours to give weights.
        List<VariableWeight> remainingBehaviours = weaponVariableDict.Values.ToList();
        int increments = 0;

        // Loop until all variables are maxed out or until totalWeight is depleted
        while (remainingBehaviours.Count > 0 && totalWeight > 0)
        {
            // Get a random value to assign weights to
            VariableWeight currentVar = GetBiasedVariable(remainingBehaviours);

            float weightForValue;

            // If the weight is too low to distribute with random, just assign the weight left
            if (totalWeight > 0.3f)
            {
                // Assign random weight
                weightForValue = UnityEngine.Random.Range(0, totalWeight / 2);
            }
            else
            {
                // Assign left over value
                weightForValue = totalWeight;
            }

            // If this is an integer value, round to top to prevent getting stuck
            if (currentVar.isIntegerValue)
            {
                weightForValue = Mathf.Ceil(weightForValue);
            }

            // Assign the new weight to the dictionary to link later to the weapon
            currentVar.currentValue += weightForValue;

            // When the value is higher than its max
            if (currentVar.currentValue > currentVar.maxedWeight)
            {
                // Subtract the difference so it can be spend on other variables
                float difference = currentVar.currentValue - currentVar.maxedWeight;
                weightForValue -= difference;
                // Clamp to max value
                currentVar.currentValue = currentVar.maxedWeight;
                // Remove the current selected value from the remaining values so it won't be selected again
                remainingBehaviours.Remove(currentVar);
            }

            // Adjust the total weight left to distribute
            totalWeight -= weightForValue;

            if (increments > 1000)
            {
                Debug.LogError("Something went terribly wrong, leading to far too many loops in CalculateWeaponVariables().");
                break;
            }
            increments++;
        }

        Debug.Log("Finished variable distribution after: " + increments + " increments");
    }

	private void CalculateWeaponVariables(LocalPlayerController player)
	{
		// Total weight to distribute at the start of generating weapons
		float totalWeight = currentBaseWeaponWeight * player.weaponWeightScalar;

		Debug.Log("Player #" + player.playerIndex + " - Variables Weight: " + totalWeight);

		// List of remaining values to give weights
		List<VariableWeight> remainingVariables = weaponVariableDict.Values.ToList();
		int increments = 0;

		// Loop until all variables are maxed out or until totalWeight is depleted
		while(remainingVariables.Count > 0 && totalWeight > 0)
		{
			// Get a random value to assign weights to
			VariableWeight currentVar = GetBiasedVariable(remainingVariables);

			float weightForValue;

			// If the weight is too low to distribute with random, just assign the weight left
			if(totalWeight > 0.3f)
			{
				// Assign random weight
				weightForValue = UnityEngine.Random.Range(0, totalWeight / 2);
			}
			else
			{
				// Assign left over value
				weightForValue = totalWeight;
			}

			// If this is an integer value, round to top to prevent getting stuck
			if(currentVar.isIntegerValue)
			{
				weightForValue = Mathf.Ceil(weightForValue);
			}

			// Assign the new weight to the dictionary to link later to the weapon
			currentVar.currentValue += weightForValue;

			// When the value is higher than its max
			if(currentVar.currentValue > currentVar.maxedWeight)
			{
				// Subtract the difference so it can be spend on other variables
				float difference = currentVar.currentValue - currentVar.maxedWeight;
				weightForValue -= difference;
				// Clamp to max value
				currentVar.currentValue = currentVar.maxedWeight;
				// Remove the current selected value from the remaining values so it won't be selected again
				remainingVariables.Remove(currentVar);
			}

			// Adjust the total weight left to distribute
			totalWeight -= weightForValue;

			if (increments > 1000)
			{
				Debug.LogError("Something went terribly wrong, leading to far too many loops in CalculateWeaponVariables().");
				break;
			}
			increments++;
		}

		Debug.Log("Finished variable distribution after: " + increments + " increments");
	}

	private VariableWeight GetRandomVariable(List<VariableWeight> possibleVars)
	{
		return possibleVars.ElementAt(UnityEngine.Random.Range(0, possibleVars.Count));
	}

	private VariableWeight GetBiasedVariable(List<VariableWeight> possibleVars)
	{
		// Plus 1 in order to get from 1 to upper (total). This makes sure distribution is equal
		int selectedNumber = UnityEngine.Random.Range(0, GetTotalBias(possibleVars)) + 1;

		int currentPassedTotalBias = 0;

		for(int i = 0; i < possibleVars.Count; i++)
		{
			currentPassedTotalBias += possibleVars[i].bias;

			if(selectedNumber <= currentPassedTotalBias)
			{
				// Found selected variable
				return possibleVars[i];
			}
		}

		Debug.LogError("Something went wrong in bias calculations as it did not find one to select");
		return null;
	}

	private int GetTotalBias(List<VariableWeight> possibleVars)
	{
		int totalVariableBias = 0;

		foreach(VariableWeight var in possibleVars)
		{
			totalVariableBias += var.bias;
		}

		return totalVariableBias;
	}

    private void ResetWeaponTimer()
	{
		refreshWeaponTimer = refreshWeaponTime;
	}
}
