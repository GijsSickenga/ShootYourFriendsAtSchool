using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
    public float offset = -0.7f;

    void Update()
    {
        transform.position = transform.parent.position + new Vector3(0, offset, 0);
        transform.rotation = Quaternion.identity;
    }
}
