using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : BehaviourProjectile
{
    // Hier alle code voor het afhandelen van de laser
    // Call zelf ergens OnTriggerBehaviour (kijk in base class)
    private float width = 5;

    public override void Initialize(BehaviourTrigger OnTriggerCallback, int playerID, BehaviourWeight settings, WeaponBehaviour.WeaponStats stats)
    {
        base.Initialize(OnTriggerCallback, playerID, settings, stats);
        Activate();
    }
    public void Activate()
    {
        //width = BehaviourSettings.LerpWeight();
        
        // Calculate where to make three raycasts (startposition with direction)
        // Make 3 raycasts (left, right, center)
        // Run raycasts
        RaycastHit2D centerCast = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity);
        RaycastHit2D[] leftCast = Physics2D.RaycastAll(transform.TransformPoint(Vector3.left * (width / 2)), transform.right, Mathf.Infinity);
        RaycastHit2D[] rightCast = Physics2D.RaycastAll(transform.TransformPoint(Vector3.right * (width / 2)), transform.right, Mathf.Infinity);

        Collider2D endCollider = new Collider2D();
        // Check center raycasts for hits (everything, players/walls)
        if(centerCast)
        {
            RaycastHit2D hit = centerCast;
            switch(hit.collider.tag)
            {
                case "untagged":
                    // untagged is the thin walls. Break out of loop, maybe particles at position?
                    // From this wall we want to bounce
                    endCollider = hit.collider;
                    break;
                
                case "Player":
                case "BreakableWall":
                    LocalPlayerController playerController = hit.collider.gameObject.GetComponent<LocalPlayerController>();
                    if(playerController)
                        if(playerController.playerIndex == PlayerID)
                            break;
                        
                    DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                    endCollider = hit.collider;
                    break;
                
                default:
                    // ignore if none of the above
                    break;
            }
        }

        // Check left raycast for hits with players/walls
        foreach(RaycastHit2D hit in leftCast)
        {
            bool exitLoop = false;
            switch(hit.collider.tag)
            {
                case "Player":
                case "BreakableWall":
                    LocalPlayerController playerController = hit.collider.gameObject.GetComponent<LocalPlayerController>();
                    if(playerController)
                        if(playerController.playerIndex == PlayerID)
                            break;

                    DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                    endCollider = hit.collider;
                    exitLoop = true;
                    break;
                
                default:
                    // ignore if none of the above
                    continue;
            }

            if(exitLoop) break;
        }

        // Check right raycast for hits with players/walls
        foreach(RaycastHit2D hit in rightCast)
        {
            bool exitLoop = false;
            switch(hit.collider.tag)
            {
                case "Player":
                case "BreakableWall":
                    LocalPlayerController playerController = hit.collider.gameObject.GetComponent<LocalPlayerController>();
                    if(playerController)
                        if(playerController.playerIndex == PlayerID)
                            break;

                    DoDamage(hit.collider.gameObject, Stats._projectileDamage);
                    endCollider = hit.collider;
                    exitLoop = true;
                    break;
                
                default:
                    // ignore if none of the above
                    continue;
            }

            if(exitLoop) break;
        }

        // Call OnTriggered only when no player got hit, give centercast's
        OnTriggerBehaviour(endCollider.transform.position, endCollider.transform.eulerAngles);
        // Make visuals
        DoVisuals(endCollider.transform.position);
    }

    private void DoVisuals(Vector3 endPosition)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(width / 2, width / 2);
        Vector3[] joe = {this.transform.position, endPosition};
        lineRenderer.SetPositions(joe);
        // Do particles at end position
        
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
