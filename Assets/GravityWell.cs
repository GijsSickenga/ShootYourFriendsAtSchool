using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public float gravity = 1.0f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.GetComponent<Rigidbody2D>().AddForce((transform.position - other.transform.position) * (gravity / Mathf.Clamp(Mathf.Abs(Vector3.Distance(other.transform.position, transform.position) * Vector3.Distance(other.transform.position, transform.position)), 1.0f, Mathf.Infinity)) * Time.deltaTime);
        }
    }
}
