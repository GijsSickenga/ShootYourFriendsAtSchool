using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBlock : MonoBehaviour {
    public int colorIndex;
    public PlayerManager playerManager;
    public int playerIndex;

    public void changeColor(int index)
    {
        colorIndex = index;

        Color theColor = playerManager.availableColors[index];
        this.GetComponent<Image>().color = theColor;
        ColorTracker.colors[playerIndex] = theColor;
    }
}
