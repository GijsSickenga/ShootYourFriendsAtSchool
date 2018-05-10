using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private LocalPlayerController playerController;

    void Start()
    {
        playerController = GetComponent<LocalPlayerController>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("FallingPlatform"))
        {
            // Make sure player is standing far enough on the platform
            if (transform.position.x + transform.lossyScale.x / 4f >= other.transform.position.x - other.transform.lossyScale.x / 2f &&
                transform.position.x - transform.lossyScale.x / 4f <= other.transform.position.x + other.transform.lossyScale.x / 2f &&
                transform.position.y + transform.lossyScale.y / 4f >= other.transform.position.y - other.transform.lossyScale.y / 2f &&
                transform.position.y - transform.lossyScale.y / 4f <= other.transform.position.y + other.transform.lossyScale.y / 2f)
            {
                playerController.standingOnPlatform = true;
            }
        }

        if (other.CompareTag("PitfallRoom"))
        {
            // Make sure player is standing far enough in the zone
            if (transform.position.x >= other.transform.position.x - other.transform.lossyScale.x / 2f &&
                transform.position.x <= other.transform.position.x + other.transform.lossyScale.x / 2f &&
                transform.position.y >= other.transform.position.y - other.transform.lossyScale.y / 2f &&
                transform.position.y <= other.transform.position.y + other.transform.lossyScale.y / 2f)
            {
                playerController.inPitfallRoom = true;
            }
        }
    }
}
