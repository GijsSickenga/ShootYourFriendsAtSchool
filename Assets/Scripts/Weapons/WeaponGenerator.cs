﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WeaponGenerator : MonoBehaviour {
	public static float currentBaseWeaponWeight = 0;
	public float weaponWeightIncrease = 5;

	public float refreshWeaponTime = 30;
	private float refreshWeaponTimer;


	public List<VariableWeight> weaponVariables;

	private Dictionary<string, VariableWeight> variableDict;

	public GameObject basicWeapon;
	public GameObject basicBullet;

	// Use this for initialization
	void Start () {
		variableDict = new Dictionary<string, VariableWeight>();

		foreach(VariableWeight var in weaponVariables)
		{
			variableDict.Add(var.variableName, var);
		}

		ResetWeaponTimer();
	}
	
	// Update is called once per frame
	void Update () {
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
			if(player != null)
			{
				// Player selected
				ResetWeaponValues();

				// Generate weapon values
				CalculateWeaponVariables();

				foreach(VariableWeight var in variableDict.Values)
				{
					Debug.Log(var.variableName + ": " + var.currentValue);
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

		while(remainingVariables.Count > 0 && totalWeight > 0)
		{
			// Get a random value to assign weights to
			VariableWeight currentVal = remainingVariables.ElementAt(UnityEngine.Random.Range(0, remainingVariables.Count));

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

			// If this is an integer value make sure there is no weights lost (or gained) because of it
			if(currentVal.isIntegerValue)
			{
				weightForValue = Mathf.Round(weightForValue);
			}


			// Assign the new weight to the dictionary to link later to the weapon
			currentVal.currentValue += weightForValue;

			if(currentVal.currentValue > currentVal.maxedWeight)
			{
				float difference = currentVal.currentValue - currentVal.maxedWeight;
				weightForValue -= difference;
				currentVal.currentValue = currentVal.maxedWeight;
			}

			// Adjust the total weight left to distribute
			totalWeight -= weightForValue;			

			// If the current value is maxed out
			if(weightForValue >= currentVal.maxedWeight)
			{
				// Remove the current selected value from the remaining values
				remainingVariables.Remove(currentVal);
			}
		}
	}

    private void ResetWeaponTimer()
	{
		refreshWeaponTimer = refreshWeaponTime;
	}
}