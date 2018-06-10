using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUITest : MonoBehaviour
{
	public List<GunUIParent> gunUIParents = new List<GunUIParent>();

	private void Start()
	{
		// Loop over all gun UI parent objects.
		foreach (GunUIParent parent in gunUIParents)
		{
			// Only show and initialize UI for players in the match.
			if (GameData.players[parent.GetPlayerID()] != null)
            {
				// Unhide UI.
				parent.gameObject.SetActive(true);

				// Initialize all sliders.
                foreach (GunUIComponent component in parent.components.Values)
                {
                    component.SetWeight(Random.Range(0, component.GetWeightedVariableType().maxedWeight));
                }
			}
		}
	}
}
