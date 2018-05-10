using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WeaponGenerator : MonoBehaviour {
	public static float currentBaseWeaponWeight = 5;

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
			variableDict.Add(var.name, var);
		}
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
		
        foreach(LocalPlayerController player in GameData.players)
		{
			if(player != null)
			{
				// Player selected
				ResetWeaponValues();

				// Generate weapon values
				CalculateWeaponVariables();

				// Link values to new weapon
				WeaponBase newWeapon = player.GiveWeapon(basicWeapon);
				newWeapon.fireRate = variableDict["FireRate"].LerpWeight();
				newWeapon.bulletDeviation = variableDict["Deviation"].LerpWeight();
				newWeapon.bulletAmount = variableDict["BulletsPerShot"].LerpWeightInt();

				player.defWeapon = newWeapon.gameObject;

				newWeapon.ammoAmount = 0;
				newWeapon.spread = 30;
			}
		}

		currentBaseWeaponWeight += 2;
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

		while(remainingVariables.Count > 0)
		{
			// Get a random value to assign weights to
			VariableWeight currentVal = remainingVariables.ElementAt(UnityEngine.Random.Range(0, remainingVariables.Count));

			// Assign random weight
			float weightForValue = UnityEngine.Random.Range(0, totalWeight);

			// If this is an integer value make sure there is no weights lost (or gained) because of it
			if(currentVal.isIntegerValue)
			{
				weightForValue = Mathf.Round(weightForValue);
			}

			// Clamp the weight to the max
			weightForValue = Mathf.Clamp(weightForValue, 0, currentVal.maxedWeight);

			// Adjust the total weight left to distribute
			totalWeight -= weightForValue;

			// Assign the new weight to the dictionary to link later to the weapon
			variableDict[currentVal.name].currentValue = weightForValue;

			// Remove the current selected value from the remaining values
			remainingVariables.Remove(currentVal);
		}
	}

    private void ResetWeaponTimer()
	{
		refreshWeaponTimer = refreshWeaponTime;
	}
}
