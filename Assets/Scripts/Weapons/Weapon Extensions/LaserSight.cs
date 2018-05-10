using UnityEngine;
using System.Collections;

public class LaserSight : MonoBehaviour {
    private LineRenderer lineRenderer;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        LocalPlayerController localPC = GetComponentInParent<LocalPlayerController>();
        if(localPC != null)
        {
            Color lineColor = ColorTracker.colors[localPC.playerIndex];
            lineRenderer.SetColors(lineColor, lineColor);
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.right);

        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, hit.point);
    }
}
