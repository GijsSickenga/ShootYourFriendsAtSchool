using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWeightScaler : MonoBehaviour {

	/*
	 *	There are a few modes, like:
	 *  Base on kill/death ratio
	 *	Base on kills in context with total kills
	 *	Base on kills away from an average
	 */

	private enum WeightBasedOn 
	{
		killDeathRatio,
		totalKills,
		averageRange
	}

	[SerializeField]
	private WeightBasedOn weightBasedOn = WeightBasedOn.averageRange;

	[SerializeField]
	// When scale is calculated, adds this minScale to calculated scale
	private float minScale = 0.5f;
	[SerializeField]
	// Maximum scale to be used
	private float maxScale = 2;

	[SerializeField]
	// Middle scale point. This should be the perfect balance between good and bad players.
	private float scaleMiddle = 1;

	// For use when needing a value under and above averages
	[SerializeField]
	private readonly int killScaleRange = 10;

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

		// Check the amount of people actually playing.
		int playingPlayers = 0;
		foreach(LocalPlayerController player in GameData.players)
		{
			// oh god why
			if(player != null)
				playingPlayers++;
		}
		// Lastly, calculate the average
		int avgKills = (int)Mathf.Ceil((float)totalKills / (float)playingPlayers);

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

					case WeightBasedOn.averageRange:
						playerData.weaponWeightScalar = CalculateAvgKillScale(playerKills, avgKills);
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
		// If no kills were made, make sure we don't divide by zero
		if(playerKills <= 0)
			playerKills++;

		// Inverse the percentage of kills player has in context of total number of kills,
		// then multiply that by the maximum scale and add the minimum scale.
		// Get lower scale for more kills
		return ((1 - (playerKills / totalKills)) * (maxScale - minScale)) + minScale;
	}

	private float CalculateKillDeathScale(float kills, float deaths)
	{
		// If no kills, add something to not divide by zero
		if (kills <= 0)
			kills++;
		// If no deaths, add something to not divide by zero
		else if (deaths <= 0)
			deaths++;

		// Inverse K/D ratio, then multiply by maxScale and add minScale. 
		// Get lower scale for higher K/D ratio.
		return (Mathf.Abs((kills / deaths) - 1) * (maxScale - minScale)) + minScale;
	}

	private float CalculateAvgKillScale(int playerKills, int avgKills)
	{
		int nKillsFromAvg = avgKills - playerKills;

		// If literally the average, return middle point 
		if(nKillsFromAvg == 0)
		{
			return scaleMiddle;
		}
		else if(nKillsFromAvg > 0)
		{
			// Check the difference between middlepoint scale and the minimum scale
			// Example: kills from average = 4, range = 10, minimum scale = 0.5, scale middlepoint = 1
			// This results in playerScale = 0.5
			float playerScale = scaleMiddle - minScale;

			// Multiply this by the inverse of player's kills multiplied by the average from the range
			// Example: 0.5 * (1 - (4 / 10)) = 0.3
			playerScale *= 1 - (nKillsFromAvg / killScaleRange);

			// If the player scale is under 0, return the minimal scale
			// Example: skip this
			if(playerScale < 0)
				return minScale;

			// Return this plus the minimal scale
			// Example: returned will be 0.3 + 0.5 = 0.7
			return playerScale + minScale;
		}
		else
		{
			// Opposite of above
			// Example: kills from average = 4, range = 10, maximum scale = 2, scale middlepoint = 1
			// This results in playerScale = 1
			float playerScale = maxScale - scaleMiddle;

			// Example: 1 * (4 / 10) = 0.4
			playerScale *= nKillsFromAvg / killScaleRange;

			// Check if playerScale is higher than 1, because it would've exceeded the range. Return maximum
			// Example: skip this
			if(playerScale > 1)
				return maxScale;

			// Example: 0.4 + 1 = 1.4
			return playerScale + scaleMiddle;
		}
	}
}