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

	// The default behaviour settings.
	public BehaviourWeight defaultBehaviourSettings;

	// Gun UI parent objects, which contain references to specific UI elements,
	// in order to adjust gun stats per player.
	// These have to be in order (0-4) for the gun stats to correspond to the right players.
	public List<GunUIParent> gunUIParents = new List<GunUIParent>();

    // Dictionaries of behaviours and variables.
    private Dictionary<string, BehaviourWeight> weaponBehaviourDict = new Dictionary<string, BehaviourWeight>();
    private Dictionary<string, VariableWeight> weaponVariableDict = new Dictionary<string, VariableWeight>();

    // Dictionary of generated behaviours to assign to weapons.
    private Dictionary<int, WeaponBehaviour> generatedBehavioursDict = new Dictionary<int, WeaponBehaviour>();

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
        foreach (BehaviourWeight var in weaponBehaviours)
        {
            weaponBehaviourDict.Add(var.variableName, var);
        }

        foreach (VariableWeight var in weaponVariables)
        {
            weaponVariableDict.Add(var.variableName, var);
        }

		ResetWeaponTimer();
	}

	private void InitializeGunUI()
	{
        // Loop over all gun UI parent objects.
        foreach (GunUIParent parent in gunUIParents)
        {
			// Only show and initialize UI for players in the match.
			if (GameData.players[parent.GetPlayerID()] != null)
			{
				// Unhide UI.
				parent.gameObject.SetActive(true);
			}
		}
	}

	/// <summary>
	/// Updates the gun UI of the player with the specified ID.
	/// </summary>
	/// <param name="playerID">The ID of the player to update the gun UI for.</param>
	private void UpdateGunUI(int playerID)
	{
        // Grab a reference to the player's gun stats UI.
        GunUIParent gunUI = gunUIParents[playerID];
        // Adjust all variable sliders.
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
        // Adjust all behaviour sliders.
        foreach (BehaviourWeight var in weaponBehaviourDict.Values)
        {
            // Grab the component corresponding to the current type.
            GunUIComponent component;
            if (gunUI.components.TryGetValue(var.variableName, out component))
            {
                // UI component found, so write the new weight on the UI element.
                component.SetWeight(var.currentValue);
            }
        }
	}
	
	private void Update()
	{
		refreshWeaponTimer -= Time.deltaTime;

		if (refreshWeaponTimer <= 0)
		{
			if(WeaponWeightScaler != null)
				WeaponWeightScaler.CalculateWeights();
            NewWeaponGeneration();
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			// Debug force generate weapons
			if(WeaponWeightScaler != null)
				WeaponWeightScaler.CalculateWeights();
            NewWeaponGeneration();
		}
	}

	/// <summary>
	/// Generates a new set of behaviours and weapon variables for all players.
	/// </summary>
	public void NewWeaponGeneration()
	{
		GenerateNewBehaviours();
		GenerateNewWeapons();
        ResetWeaponTimer();
        currentBaseWeaponWeight += weaponWeightIncrease;
	}

	/// <summary>
	/// Generates weapon behaviour chains for all players.
	/// </summary>
	public void GenerateNewBehaviours()
	{
        Debug.Log("New behaviours generated with weight of: " + currentBaseWeaponWeight);

        // Empty the generated behaviours dictionary to make room for new ones.
        generatedBehavioursDict.Clear();

		// Generate a behaviour chain for each player in the game.
        foreach (LocalPlayerController player in GameData.players)
        {
            if (player != null)
            {
                // Reset values prior to generating new ones.
                ResetValues(weaponBehaviourDict);

                // Generate behaviour values.
                Queue<BehaviourWeight> behavioursToGenerate = CalculateBehaviours(player);

                // The base behaviour to assign to this player's weapon later.
                // Assign default shooting behaviour in case no specific behaviours were selected.
                WeaponBehaviour baseBehaviour = new DefaultBehaviour(player.playerIndex, defaultBehaviourSettings, null);

				// Make sure at least one behaviour was selected.
				if (behavioursToGenerate.Count > 0)
                {
                    // Generate ordered behaviour chain from selected behaviours.
                    baseBehaviour = GenerateBehaviourChain(player.playerIndex, behavioursToGenerate);
				}

                // Cache behaviour with player ID until weapons are generated and behaviours can be attached.
                generatedBehavioursDict.Add(player.playerIndex, baseBehaviour);
            }
        }
	}

	/// <summary>
	/// Generates a new behaviour chain from a queue of selected behaviours.
	/// </summary>
	private WeaponBehaviour GenerateBehaviourChain(int playerID, Queue<BehaviourWeight> selectedBehaviours)
	{
		// Grab the behaviour to generate in this loop.
		BehaviourWeight behaviourSettings = selectedBehaviours.Dequeue();
		// Check which type of behaviour we are generating.
		// This is used to instantiate a new behaviour of that type.
		Type behaviourType = behaviourSettings.behaviourScript.GetType();

		// Check if there are any behaviours left to generate after the current one.
		if (selectedBehaviours.Count > 0)
        {
			// Generate behaviour and move to the next behaviour in the chain.
            return (WeaponBehaviour)Activator.CreateInstance(behaviourType, playerID, behaviourSettings, GenerateBehaviourChain(playerID, selectedBehaviours));
		}
		else
		{
			// Generate the final behaviour in the chain.
			return (WeaponBehaviour)Activator.CreateInstance(behaviourType, playerID, behaviourSettings, null);
		}
	}

    public void GenerateNewWeapons()
    {
		Debug.Log("New weapons generated with weight of: " + currentBaseWeaponWeight);
        foreach (LocalPlayerController player in GameData.players)
		{
			if (player != null)
			{
				// Player selected
				ResetValues(weaponVariableDict);

				// Generate weapon values
				CalculateWeaponVariables(player);

				// Update gun UI for current player to new values.
				UpdateGunUI(player.playerIndex);

				// Link values to new weapon
				WeaponBase newWeapon = player.GiveDefaultWeapon();
				newWeapon.fireRate = weaponVariableDict["Fire Rate"].LerpWeight();
				newWeapon.bulletDeviation = weaponVariableDict["Deviation"].LerpWeight();
				newWeapon.bulletAmount = weaponVariableDict["Bullets Per Shot"].LerpWeightInt();

				newWeapon.bulletSpeed = weaponVariableDict["Bullet Speed"].LerpWeight();
				newWeapon.bulletDamage = weaponVariableDict["Damage"].LerpWeightInt();
				newWeapon.bulletColor = ColorTracker.colors[player.playerIndex];

				newWeapon.ammoAmount = 0;
				newWeapon.spread = 30;

				// Assign behaviour chain to weapon.
				newWeapon.ShootingBehaviour = generatedBehavioursDict[player.playerIndex];
			}
		}
    }

	public void FirstGenerate()
	{
		if (!hasGeneratedFirst)
        {
            GenerateNewBehaviours();
			GenerateNewWeapons();
			hasGeneratedFirst = true;
		}

        // Activate gun UI for all joined players.
        InitializeGunUI();
    }

    private void ResetValues(Dictionary<string, VariableWeight> variableWeights)
    {
        foreach (KeyValuePair<string, VariableWeight> pair in variableWeights)
        {
            pair.Value.currentValue = 0;
        }
    }
    private void ResetValues(Dictionary<string, BehaviourWeight> behaviourWeights)
    {
        foreach (KeyValuePair<string, BehaviourWeight> pair in behaviourWeights)
        {
            pair.Value.currentValue = 0;
			pair.Value.enqueued = false;
        }
    }

	/// <summary>
	/// Returns a list of all behaviours whose threshold is lower than the
	/// specified total weight.
	/// </summary>
	List<BehaviourWeight> GetListOfPossibleBehaviours(float totalWeight)
	{
        // List of all behaviours.
        List<BehaviourWeight> possibleBehaviours = weaponBehaviourDict.Values.ToList();

        // Filter out behaviours that have a higher threshold value than the specified max weight.
        for (int i = possibleBehaviours.Count - 1; i > -1; i--)
        {
            if (possibleBehaviours[i].thresholdWeight > totalWeight)
            {
				// Make sure we aren't adding maxed out behaviours to the list.
				if (possibleBehaviours[i].currentValue < possibleBehaviours[i].maxedWeight)
                {
                    possibleBehaviours.Remove(possibleBehaviours[i]);
				}
            }
        }

		// Return remaining behaviours.
		return possibleBehaviours;
	}

    private Queue<BehaviourWeight> CalculateBehaviours(LocalPlayerController player)
    {
		// Queue of selected behaviour names.
		Queue<string> selectedBehaviourNames = new Queue<string>();
		Queue<BehaviourWeight> selectedBehaviours = new Queue<BehaviourWeight>();
		
        // Total weight to distribute at the start of generating behaviours.
        float totalWeight = currentBaseWeaponWeight * player.weaponWeightScalar;

        Debug.Log("Player #" + player.playerIndex + " - Behaviours Weight: " + totalWeight);

        int increments = 0;

		// Get a list of all behaviours within the player's weight range.
		List<BehaviourWeight> remainingBehaviours = GetListOfPossibleBehaviours(totalWeight);

        // Loop until all behaviours are maxed out or until totalWeight is depleted.
        while (remainingBehaviours.Count > 0 && totalWeight > 0)
        {
            // Get a random behaviour to assign weights to.
            BehaviourWeight currentBehaviour = (BehaviourWeight)GetBiasedVariable(remainingBehaviours.ToArray());

            float weightForValue;

            // If the weight is too low to distribute with random, just assign the remaining weight.
            if (totalWeight > 0.3f)
            {
                // Assign random weight.
				// TODO: experiment with different dividers here.
                weightForValue = UnityEngine.Random.Range(0, totalWeight / 2);
            }
            else
            {
                // Assign left over value.
                weightForValue = totalWeight;

                // Divide leftover weight in unenqueued behaviours over enqueued behaviours,
                // to prevent having a bunch of weight sitting unused in unenqueued behaviours.
                for (int i = remainingBehaviours.Count - 1; i > -1; i--)
                {
                    if (!remainingBehaviours[i].enqueued)
                    {
                        totalWeight += remainingBehaviours[i].currentValue;
                        remainingBehaviours[i].currentValue = 0;
                        remainingBehaviours.Remove(remainingBehaviours[i]);
                    }
                }

				// If we removed all behaviours, redo loop with remaining behaviours
				// that could reach their threshold.
				if (remainingBehaviours.Count == 0)
				{
                    remainingBehaviours = GetListOfPossibleBehaviours(totalWeight);
				}
            }

            // If this is an integer value, round to top to prevent getting stuck.
            if (currentBehaviour.isIntegerValue)
            {
                weightForValue = Mathf.Ceil(weightForValue);
            }

            // Assign the new weight to the dictionary entry to link later to the behaviour.
            currentBehaviour.currentValue += weightForValue;

            // When the value is higher than its max.
            if (currentBehaviour.currentValue > currentBehaviour.maxedWeight)
            {
                // Subtract the difference so it can be spent on other variables.
                float difference = currentBehaviour.currentValue - currentBehaviour.maxedWeight;
                weightForValue -= difference;
                // Clamp to max value.
                currentBehaviour.currentValue = currentBehaviour.maxedWeight;
                // Remove the current selected value from the remaining values so it won't be selected again.
                remainingBehaviours.Remove(currentBehaviour);
            }

            // Adjust the total weight left to distribute.
            totalWeight -= weightForValue;

			// Make sure the current behaviour isn't already enqueued.
			if (!currentBehaviour.enqueued)
			{
				// If the current behaviour has reached its threshold value, enqueue it
				// in the queue of selected behaviours.
				if (currentBehaviour.currentValue >= currentBehaviour.thresholdWeight)
				{
                    selectedBehaviourNames.Enqueue(currentBehaviour.variableName);
                    currentBehaviour.enqueued = true;
				}
			}

            if (increments > 1000)
            {
                Debug.LogError("Something went terribly wrong, leading to far too many loops in CalculateBehaviours().");
                break;
            }
            increments++;
        }

        Debug.Log("Finished behaviour distribution after: " + increments + " increments");

		// Generate queue of actual behaviours from the queue of behaviour names.
		while (selectedBehaviourNames.Count > 0)
		{
			selectedBehaviours.Enqueue(weaponBehaviourDict[selectedBehaviourNames.Dequeue()]);
		}

		// Return queue of selected behaviours.
		return selectedBehaviours;
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
		while (remainingVariables.Count > 0 && totalWeight > 0)
		{
			// Get a random value to assign weights to
			VariableWeight currentVar = GetBiasedVariable(remainingVariables.ToArray());

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

            // Assign the new weight to the dictionary entry to link later to the weapon
            currentVar.currentValue += weightForValue;

			// When the value is higher than its max
			if(currentVar.currentValue > currentVar.maxedWeight)
			{
				// Subtract the difference so it can be spent on other variables
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

    private VariableWeight GetBiasedVariable(VariableWeight[] possibleVars)
    {
        // Plus 1 in order to get from 1 to upper (total). This makes sure distribution is equal
        int selectedNumber = UnityEngine.Random.Range(0, GetTotalBias(possibleVars)) + 1;

        int currentPassedTotalBias = 0;

        for (int i = 0; i < possibleVars.Count(); i++)
        {
            currentPassedTotalBias += possibleVars[i].bias;

            if (selectedNumber <= currentPassedTotalBias)
            {
                // Found selected variable
                return possibleVars[i];
            }
        }

        Debug.LogError("Something went wrong in bias calculations as it did not find one to select.");
        return null;
    }

	private int GetTotalBias(VariableWeight[] possibleVars)
	{
		int totalVariableBias = 0;

		foreach (VariableWeight var in possibleVars)
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
