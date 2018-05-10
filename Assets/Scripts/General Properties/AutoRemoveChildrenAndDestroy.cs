using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoRemoveChildrenAndDestroy : MonoBehaviour
{
    void Start()
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            child.parent = null;
        }

        Destroy(gameObject);
    }
}
