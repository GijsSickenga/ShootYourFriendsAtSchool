using UnityEngine;
using System.Collections;

public class ArrowTrapGizmo : MonoBehaviour
{
    public ArrowTrap trap;

    public Color unselectedColor = new Color(0, 1, 168f / 255f);

    void OnDrawGizmos()
    {
        RaycastHit2D hit = Physics2D.Raycast(trap.projectileSpawn.position, transform.right);

        Gizmos.color = unselectedColor;

        // Line showing the trap's activation raycast
        Gizmos.DrawLine(trap.projectileSpawn.position, hit.point);

        // Lines showing the trap's spread
        if (trap.spread > 0)
        {
            RaycastHit2D spreadTopHit = Physics2D.Raycast(trap.projectileSpawn.position,
                                                          transform.right + new Vector3(Mathf.Cos((transform.eulerAngles.z + trap.spread) * Mathf.Deg2Rad),
                                                                                        Mathf.Sin((transform.eulerAngles.z + trap.spread) * Mathf.Deg2Rad),
                                                                                        0));

            RaycastHit2D spreadBottomHit = Physics2D.Raycast(trap.projectileSpawn.position,
                                                             transform.right + new Vector3(Mathf.Cos((transform.eulerAngles.z - trap.spread) * Mathf.Deg2Rad),
                                                                                           Mathf.Sin((transform.eulerAngles.z - trap.spread) * Mathf.Deg2Rad),
                                                                                           0));
            Gizmos.color = new Color(unselectedColor.r * 0.75f,
                                     unselectedColor.g * 0.75f,
                                     unselectedColor.b * 0.75f,
                                     unselectedColor.a * 0.75f);

            Gizmos.DrawLine(trap.projectileSpawn.position, spreadTopHit.point);
            Gizmos.DrawLine(trap.projectileSpawn.position, spreadBottomHit.point);
        }
    }
}
