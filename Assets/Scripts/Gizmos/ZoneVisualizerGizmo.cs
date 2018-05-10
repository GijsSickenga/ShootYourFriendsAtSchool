using UnityEngine;
using System.Collections;

public class ZoneVisualizerGizmo : MonoBehaviour
{
    public Color unselectedColor = Color.red;
    public Color selectedColor = Color.cyan;
    public bool hatchedWhileUnselected = false;
    public bool hatchedWhileSelected = true;
    public int hatchDensity = 4;

    void OnDrawGizmos()
    {
        Gizmos.color = unselectedColor;

        if (hatchedWhileUnselected)
        {
            GizmoRectangle.DrawHatched(transform.position, transform.lossyScale.x, transform.lossyScale.y, hatchDensity);
        }
        else
        {
            GizmoRectangle.Draw(transform.position, transform.lossyScale.x, transform.lossyScale.y);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = selectedColor;

        if (hatchedWhileSelected)
        {
            GizmoRectangle.DrawHatched(transform.position, transform.lossyScale.x, transform.lossyScale.y, hatchDensity);
        }
        else
        {
            GizmoRectangle.Draw(transform.position, transform.lossyScale.x, transform.lossyScale.y);
        }
    }
}
