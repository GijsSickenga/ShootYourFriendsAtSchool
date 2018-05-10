using UnityEngine;
using System.Collections;

public class WeaponSpawnGizmo : MonoBehaviour
{
    public Color selectedColor = new Color(1f, 1f, 0f);
    public Color unselectedColor = new Color(1f, 75f/255f, 0f);
    public float radius = 1f;

    void OnDrawGizmos()
    {
        // Draw boxed in sphere
        Gizmos.color = unselectedColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void OnDrawGizmosSelected()
    {
        // Draw boxed in sphere
        Gizmos.color = selectedColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
