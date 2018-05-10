using UnityEngine;
using System.Collections;

public class PlayerSpawnGizmo : MonoBehaviour
{
    public Color selectedColor = new Color(1, 0, 100f / 255f);
    public Color unselectedColor = new Color(0, 1, 168f / 255f);
    public float radius = 1f;

    void OnDrawGizmos()
    {
        Vector3 start = new Vector3(-Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * radius,
                                    -Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * radius,
                                    0);
        Vector3 end = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * 2f * radius,
                                  Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * 2f * radius,
                                  0);
        
        // Draw icon
        Gizmos.color = unselectedColor;
        Gizmos.DrawWireSphere(transform.position, radius);

        DrawArrow(transform.position + start, transform.position + end);

        Gizmos.DrawLine(Quaternion.Euler(0, 0, 90) * start + transform.position,
                        Quaternion.Euler(0, 0, 90) * -start + transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 start = new Vector3(-Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * radius,
                                    -Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * radius,
                                    0);
        Vector3 end = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * 2f * radius,
                                  Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * 2f * radius,
                                  0);

        // Draw icon
        Gizmos.color = selectedColor;
        Gizmos.DrawWireSphere(transform.position, radius);

        DrawArrow(transform.position + start, transform.position + end);

        Gizmos.DrawLine(Quaternion.Euler(0, 0, 90) * start + transform.position,
                        Quaternion.Euler(0, 0, 90) * -start + transform.position);
    }

    void DrawArrow(Vector3 start, Vector3 end)
    {
        Gizmos.DrawLine(start, end);
        Gizmos.DrawLine(end, end + new Vector3(Mathf.Cos((transform.eulerAngles.z + 135) * Mathf.Deg2Rad) * radius,
                                               Mathf.Sin((transform.eulerAngles.z + 135) * Mathf.Deg2Rad) * radius,
                                               0));
        Gizmos.DrawLine(end, end + new Vector3(Mathf.Cos((transform.eulerAngles.z - 135) * Mathf.Deg2Rad) * radius,
                                               Mathf.Sin((transform.eulerAngles.z - 135) * Mathf.Deg2Rad) * radius,
                                               0));
    }
}
