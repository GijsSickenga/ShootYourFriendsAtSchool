using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
    public float amount = 1.0f;

	void Update () {
        transform.Rotate(0, 0, amount);
	}
}
