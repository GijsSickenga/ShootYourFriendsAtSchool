using UnityEngine;
using System.Collections;

public class GizmoWeaponSpawnpoint : MonoBehaviour {
    public float radius = 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, radius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position, radius);
    }
}
