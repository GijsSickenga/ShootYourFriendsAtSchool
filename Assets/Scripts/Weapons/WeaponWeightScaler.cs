using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWeightScaler : MonoBehaviour {

	/*
	 *	There are a few modes, like:
	 *  Base on kill/death ratio
	 *	Base on kills in context with total kills
	 */

	private enum WeightBasedOn 
	{
		killDeathRatio,
		totalKills
	}

	[SerializeField]
	private WeightBasedOn weightBasedOn = WeightBasedOn.totalKills;

	[SerializeField]
	// When scale is calculated, adds this minScale to calculated scale
	private float minScale = 0;
	[SerializeField]
	// Maximum scale to be used
	private float maxScale = 2;

	// Calculates weights for each player
	public void CalculateWeights()
	{
		// cus of old systems, hardcoded arrays are in place. gut code.
		int[] killsArray = {GameController.p1Kills, GameController.p2Kills, GameController.p3Kills, GameController.p4Kills};
		int[] deathsArray = {GameController.p1Deaths, GameController.p2Deaths, GameController.p3Deaths, GameController.p4Deaths};

		// Tally up all kills and deaths made to compare later
		int totalKills = 0;
		int totalDeaths = 0;
		for(int i = 0; i < killsArray.Length; i++)
		{
			totalKills += killsArray[i];
			totalDeaths += deathsArray[i];
		}

		if(totalKills <= 0 || totalDeaths <= 0)
			return;
		
		// Now for each player, let's see how they've done
		for(int i = 0; i < GameData.players.Length; i++)
		{
			// Get local variable for player's data, and check if not null
			LocalPlayerController playerData = GameData.players[i];
			if(playerData != null)
			{
				// More local variables to not get confused later on
				int playerKills = killsArray[i];
				int playerDeaths = deathsArray[i];

				// Switch based on what we want to calculate scale from
				switch(weightBasedOn)
				{
					case WeightBasedOn.totalKills:
						playerData.weaponWeightScalar = CalculateKillRatioScale(playerKills, totalKills);
						break;
					
					case WeightBasedOn.killDeathRatio:
						playerData.weaponWeightScalar = CalculateKillDeathScale(playerKills, playerDeaths);
						break;

					default:
						break;
				}

				Debug.Log("Player #" + playerData.playerIndex + " - Scale: " + playerData.weaponWeightScalar + " - Kills: " + playerKills);
			}
		}
	}

	private float CalculateKillRatioScale(float playerKills, float totalKills)
	{
		// If no kills were made, return maximum scale
		if(playerKills <= 0)
			return maxScale;

		// Inverse the percentage of kills player has in context of total number of kills,
		// then multiply that by the maximum scale and add the minimum scale.
		// Get lower scale for more kills
		return (Mathf.Abs((playerKills / totalKills) - 1) * (maxScale - minScale)) + minScale;
	}

	private float CalculateKillDeathScale(float kills, float deaths)
	{
		// If no kills, return maximum scale
		if (kills <= 0)
			return maxScale;
		// If no deaths, return minimum scale
		else if (deaths <= 0)
			return 0 + minScale;

		// Inverse K/D ratio, then multiply by maxScale and add minScale. 
		// Get lower scale for higher K/D ratio.
		return (Mathf.Abs((kills / deaths) - 1) * (maxScale - minScale)) + minScale;
	}
}
