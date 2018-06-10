using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUIParent : MonoBehaviour
{
    /// <summary>
    /// A list of gun UI components contained in this gun UI parent.
    /// </summary>
    [SerializeField]
    private List<GunUIComponent> componentsList = new List<GunUIComponent>();

    // The player id associated with this UI group.
    [SerializeField]
    private int playerID = -1;
    public int GetPlayerID()
    {
        return playerID;
    }

    // The title text of the UI group, based on player ID.
    [SerializeField]
    private Text titleText;

    /// <summary>
    /// A dictionary containing all gun UI components with their
    /// corresponding gun variable types.
    /// </summary>
    [HideInInspector]
    public Dictionary<string, GunUIComponent> components = new Dictionary<string, GunUIComponent>();

    // Initialization.
    private void Awake()
    {
        foreach (GunUIComponent component in componentsList)
        {
            // Initialize slider color by player ID.
            component.SetSliderColor(ColorTracker.colors[playerID]);

            // Add the component to the dictionary, along with its weighted
            // variable type as its ID.
            components.Add(component.GetWeightedVariableType().variableName, component);
        }
    }

    // Update UI elements.
    private void OnValidate()
    {
        if (playerID != -1)
        {
            // Change gameobject name and title based on player ID.
            gameObject.name = "Player " + (playerID + 1) + " Gun UI";

            if (titleText != null)
            {
                titleText.text = "Player " + (playerID + 1) + " Gun Stats";
            }
        }
    }
}
