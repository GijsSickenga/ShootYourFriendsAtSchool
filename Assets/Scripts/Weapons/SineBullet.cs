using UnityEngine;
using System.Collections;

public class SineBullet : BulletBase
{
    public float amplitude, oscillationSpeed;
    private float oscillation;
    private Vector3 realPosition;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        realPosition = transform.position;
    }

    void Update()
    {
        oscillation += oscillationSpeed;
        realPosition += new Vector3(body.velocity.x, body.velocity.y, realPosition.z) * Time.deltaTime;
        transform.position = new Vector3(realPosition.x + Mathf.Sin(oscillation) * amplitude,
                                         realPosition.y,
                                         realPosition.z);
    }
}
