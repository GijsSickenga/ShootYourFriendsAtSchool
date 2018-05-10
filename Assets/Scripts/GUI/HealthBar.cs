using UnityEngine;
using System.Collections;

// WIP CLASS - probably rafactor this to a MonoBehavior so it can easily be added to objects in the editor
public class HealthBar
{
    private Rect myRect;

    private Color currentColor;
    private Color[] colorPalette;

    private float healthThresholds;

    public HealthBar(Rect rect, Color color)
    {
        myRect = rect;

        currentColor = color;
    }

    // Call this constructor when you want a healthbar with multiple colors depending on health
    public HealthBar(Rect rect, Color[] palette, float healthThresholds)
    {
        myRect = rect;

        colorPalette = palette;
        currentColor = palette[0];
    }

    public void Update()
    {

    }

    // Draw a colored rectangle on the screen
    // Only call from OnGUI()
    public void GUIDrawRect()
    {
        Texture2D texture = new Texture2D(1, 1);
        GUIStyle style = new GUIStyle();

        // Color the texture
        texture.SetPixel(0, 0, currentColor);
        texture.Apply();
        style.normal.background = texture;

        // Draw the rectangle
        GUI.Box(myRect, GUIContent.none, style);
    }
}
