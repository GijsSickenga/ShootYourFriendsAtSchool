using UnityEngine;
using System.Collections;

public class LaserSword : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = ColorTracker.colors[GetComponentInParent<LocalPlayerController>().playerIndex];
    }
}
