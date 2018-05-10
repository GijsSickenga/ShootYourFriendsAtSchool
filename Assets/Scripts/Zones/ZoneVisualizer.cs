using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneVisualizer : MonoBehaviour
{
    public Color startColor = Color.magenta;
    public float thickness = 0.1f;

    private List<GameObject> lines = new List<GameObject>();

    void Start()
    {
        Vector3 position = transform.position;
        float width = transform.lossyScale.x;
        float height = transform.lossyScale.y;

        // Left line
        GameObject leftLine = new GameObject("Left Line");
        InitializeLine(leftLine,
                       new Vector3(position.x + thickness / 2, position.y, position.z),
                       new Vector3(position.x + thickness / 2, position.y - height, position.z),
                       thickness);
        lines.Add(leftLine);

        // Top line
        GameObject topLine = new GameObject("Top Line");
        InitializeLine(topLine,
                       new Vector3(position.x + thickness, position.y - thickness / 2, position.z),
                       new Vector3(position.x + width - thickness, position.y - thickness / 2, position.z),
                       thickness);
        lines.Add(topLine);
            
        // Right line
        GameObject rightLine = new GameObject("Right Line");
        InitializeLine(rightLine,
                       new Vector3(position.x + width - thickness / 2, position.y, position.z),
                       new Vector3(position.x + width - thickness / 2, position.y - height, position.z),
                       thickness);
        lines.Add(rightLine);

        // Bottom line
        GameObject bottomLine = new GameObject("Bottom Line");
        InitializeLine(bottomLine,
                       new Vector3(position.x + thickness, position.y - height + thickness / 2, position.z),
                       new Vector3(position.x + width - thickness, position.y - height + thickness / 2, position.z),
                       thickness);
        lines.Add(bottomLine);
    }

    void InitializeLine(GameObject line, Vector3 startPosition, Vector3 endPosition, float thickness = 0.1f)
    {
        line.transform.position = transform.position;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(startColor, startColor);
        lr.SetWidth(thickness, thickness);
        lr.SetPosition(0, startPosition);
        lr.SetPosition(1, endPosition);
        lr.sortingLayerName = "Zone Visualizer";
    }

    public void ChangeColor(Color color)
    {
        foreach (GameObject line in lines)
        {
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.SetColors(color, color);
        }
    }
}
