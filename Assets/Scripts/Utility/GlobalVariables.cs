using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System;

public static class GlobalVariables
{
    private static string[] ignoreCollisionTagArray = { "Bullet", "Zone", "Pick up", "LowWall", "FloorTrap", "FallingPlatform", "PitfallRoom", "Lever", "HealthPickUp", "GravityWell", "VoidBullet" };

    public static ReadOnlyCollection<string> IgnoreCollisionTags
    {
        get
        {
            return Array.AsReadOnly(ignoreCollisionTagArray);
        }
    }
}
