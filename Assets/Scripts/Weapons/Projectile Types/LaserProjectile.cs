using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : BehaviourProjectile
{
    // Hier alle code voor het afhandelen van de laser
    // Call zelf ergens OnTriggerBehaviour (kijk in base class)
    private float width = 5;
    private readonly float LASER_LIFETIME = 0.5f;

    public override void Initialize(BehaviourTrigger OnTriggerCallback, int playerID, BehaviourWeight settings, WeaponBehaviour.WeaponStats stats)
    {
        base.Initialize(OnTriggerCallback, playerID, settings, stats);
        Activate();
    }
    public void Activate()
    {
        width = BehaviourSettings.LerpWeight();
        
        // Calculate where to make three raycasts (startposition with direction)
        // Make 3 raycasts (left, right, center)
        // Run raycasts
        RaycastHit2D centerCast = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity);
        RaycastHit2D[] leftCast = Physics2D.RaycastAll(transform.TransformPoint(Vector3.left * (width / 2)), transform.right, Mathf.Infinity);
        RaycastHit2D[] rightCast = Physics2D.RaycastAll(transform.TransformPoint(Vector3.right * (width / 2)), transform.right, Mathf.Infinity);

        Collider2D endCollider = new Collider2D();
        Vector2 endPoint = Vector2.zero;
        // Check center raycasts for hits (everything, players/walls)
        if(centerCast)
        {
            RaycastHit2D hit = centerCast;
            if(hit.collider.tag == "Untagged")
            {
                endCollider = hit.collider;
                endPoint = hit.point;
            }
            else if(hit.collider.tag == "Player")
            {
                LocalPlayerController playerController = hit.collider.gameObject.GetComponent<LocalPlayerController>();
                if(playerController.playerIndex != PlayerID)
                {
                    DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                    endCollider = hit.collider;
                    endPoint = hit.point;
                }
            }
            else if(hit.collider.tag == "BreakableWall")
            {
                DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                endCollider = hit.collider;
                endPoint = hit.point;
            }
        }

        // Check left raycast for hits with players/walls
        foreach(RaycastHit2D hit in leftCast)
        {
            bool exitLoop = false;
            if(hit.collider.tag == "Player")
            {
                LocalPlayerController playerController = hit.collider.gameObject.GetComponent<LocalPlayerController>();
                if(playerController.playerIndex != PlayerID)
                {
                    DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                    endCollider = hit.collider;
                    endPoint = hit.point;
                }
                exitLoop = true;
            }
            else if(hit.collider.tag == "BreakableWall")
            {
                DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                endCollider = hit.collider;
                endPoint = hit.point;
                exitLoop = true;
            }

            if(exitLoop) break;
        }

        // Check right raycast for hits with players/walls
        foreach(RaycastHit2D hit in rightCast)
        {
            bool exitLoop = false;
            if(hit.collider.tag == "Player")
            {
                LocalPlayerController playerController = hit.collider.gameObject.GetComponent<LocalPlayerController>();
                if(playerController.playerIndex != PlayerID)
                {
                    DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                    endCollider = hit.collider;
                    endPoint = hit.point;
                }
                exitLoop = true;
            }
            else if(hit.collider.tag == "BreakableWall")
            {
                DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                endCollider = hit.collider;
                endPoint = hit.point;
                exitLoop = true;
            }

            if(exitLoop) break;
        }

        // Why? Because welcome back to C#.
        if(endCollider == null)
        {
            Destroy(gameObject);
            return;
        }

        if(endCollider.transform == null)
        {
            Destroy(gameObject);
            return;
        }

        // Make visuals
        StartCoroutine(DoVisuals(endPoint));
        Destroy(gameObject, LASER_LIFETIME);

        // Nothing with health hit
        RaycastHit2D rayCast;
        Vector2 vel = endPoint;
        rayCast = Physics2D.Raycast(transform.position, vel);
        
        Vector2 reflected = Vector2.Reflect(vel, rayCast.normal);
        reflected.Normalize();
        float angle = Mathf.Atan2(reflected.y, reflected.x);

        Vector3 newAngle = new Vector3(0, 0, Mathf.Rad2Deg * angle);
        // Call OnTriggered only when no player got hit, give centercast's
        OnTriggerBehaviour(endPoint, newAngle, this);
    }

    private IEnumerator DoVisuals(Vector3 endPosition)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Stats._projectileColor;
        lineRenderer.endColor = Stats._projectileColor;
        lineRenderer.SetWidth(width / 2, width / 2);
        Vector3[] joe = {this.transform.position, endPosition};
        lineRenderer.SetPositions(joe);

        while(lineRenderer.startColor.a > 0)
        {
            Color lineColor = lineRenderer.startColor;
            lineColor.a -= LASER_LIFETIME * Time.deltaTime;
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            yield return null;
        }

    }

    private void DoDamage(GameObject damagedPlayer, int damage)
    {
        Health health = damagedPlayer.GetComponent<Health>();
        Breakable breakHealth = damagedPlayer.GetComponent<Breakable>();

        if (health != null)
        {
            //Giving lastHit the playerIndex of the shooting player
            health.lastHit = PlayerID;
            health.ResetLastHitTimer();
            health.Damage(damage);
        }
        else if (breakHealth != null)
        {
            breakHealth.Damage(damage);
        }
    }

    void OnDrawGizmos()
    {
        // Draw raycasts to check if they go right
        Gizmos.color = new Color(255, 255, 255, 255);
        Gizmos.DrawRay(new Ray(transform.position, transform.eulerAngles));
        Gizmos.DrawRay(new Ray(transform.TransformPoint(Vector3.left * (width / 2)), transform.eulerAngles));
        Gizmos.DrawRay(new Ray(transform.TransformPoint(Vector3.right * (width / 2)), transform.eulerAngles));
    }
}
