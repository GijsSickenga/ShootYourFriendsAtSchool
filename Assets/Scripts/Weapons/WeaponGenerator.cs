﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
	public static float currentBaseWeaponWeight = 0;
	public float weaponWeightIncrease = 5;

	public float refreshWeaponTime = 30;
	private float refreshWeaponTimer;

	public List<VariableWeight> weaponVariables = new List<VariableWeight>();

	// Gun UI parent objects, which contain references to specific UI elements,
	// in order to adjust gun stats per player.
	// These have to be in order (0-4) for the gun stats to correspond to the right players.
	public List<GunUIParent> gunUIParents = new List<GunUIParent>();

    private Dictionary<string, VariableWeight> variableDict = new Dictionary<string, VariableWeight>();

    private void Start()
    {
		// Initialize dictionary from list.
        foreach (VariableWeight var in weaponVariables)
        {
            variableDict.Add(var.variableName, var);
        }

		ResetWeaponTimer();
	}
	
	private void Update()
	{
		refreshWeaponTimer -= Time.deltaTime;

		if(refreshWeaponTimer <= 0)
		{
			GenerateNewWeapons();
			ResetWeaponTimer();
		}
	}

    public void GenerateNewWeapons()
    {
		Debug.Log("New weapons generated with weight of: " + currentBaseWeaponWeight);
        foreach(LocalPlayerController player in GameData.players)
		{
			if (player != null)
			{
				// Player selected
				ResetWeaponValues();

				// Generate weapon values
				CalculateWeaponVariables();

				// Grab a reference to this player's gun stats UI.
                GunUIParent gunUI = gunUIParents[player.playerIndex];
				// Adjust all gun stat values in UI.
				foreach(VariableWeight var in variableDict.Values)
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
				WeaponBase newWeapon = player.defWeapon.GetComponent<WeaponBase>();
				newWeapon.fireRate = variableDict["FireRate"].LerpWeight();
				newWeapon.bulletDeviation = variableDict["Deviation"].LerpWeight();
				newWeapon.bulletAmount = variableDict["BulletsPerShot"].LerpWeightInt();

				BulletBase newBullet = newWeapon.bulletPrefab.GetComponent<BulletBase>();
				newBullet.bulletSpeed = variableDict["BulletSpeed"].LerpWeight();
				newBullet.damage = variableDict["Damage"].LerpWeightInt();

				player.GiveDefaultWeapon();

				newWeapon.ammoAmount = 0;
				newWeapon.spread = 30;
			}
		}

		currentBaseWeaponWeight += weaponWeightIncrease;
    }

	private void ResetWeaponValues()
	{
		foreach(KeyValuePair<string, VariableWeight> pair in variableDict)
		{
			pair.Value.currentValue = 0;
		}
	}

	private void CalculateWeaponVariables()
	{
		// Total weight to distribute at the start of generating weapons
		float totalWeight = currentBaseWeaponWeight;

		// List of remaining values to give weights
		List<VariableWeight> remainingVariables = variableDict.Values.ToList();
		int increments = 0;

		// Loop until all variables are maxed out or until totalWeight is depleted
		while(remainingVariables.Count > 0 && totalWeight > 0)
		{
			// Get a random value to assign weights to
			VariableWeight currentVar = remainingVariables.ElementAt(UnityEngine.Random.Range(0, remainingVariables.Count));

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

    private void ResetWeaponTimer()
	{
		refreshWeaponTimer = refreshWeaponTime;
	}
}
