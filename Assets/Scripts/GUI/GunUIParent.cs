using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUIParent : MonoBehaviour
{
    /// <summary>
    /// A list of gun UI components contained in this gun UI parent.
    /// </summary>
    [SerializeField]
    private List<GunUIComponent> componentsList = new List<GunUIComponent>();

    // The player id associated with this UI group.
    [SerializeField]
    private int playerID;

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
            component.SetSliderColor(playerID);

            // Add the component to the dictionary, along with its weighted
            // variable type as its ID.
            components.Add(component.GetWeightedVariableType().variableName, component);
        }
    }
}
