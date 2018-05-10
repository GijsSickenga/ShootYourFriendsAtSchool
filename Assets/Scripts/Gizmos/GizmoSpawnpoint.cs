using UnityEngine;
using System.Collections;

public class GizmoSpawnpoint : MonoBehaviour {
    public float radius = 0.2f;

	void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, radius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position, radius);
    }
}
