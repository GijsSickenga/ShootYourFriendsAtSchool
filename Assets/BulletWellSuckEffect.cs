using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWellSuckEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private float suckTimer = 1.0f;
    public float startAlpha;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startAlpha = spriteRenderer.color.a;
        Destroy(gameObject, suckTimer);
    }

    void Update()
    {
        suckTimer -= Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r,
                                         spriteRenderer.color.g,
                                         spriteRenderer.color.b,
                                         spriteRenderer.color.a - startAlpha / suckTimer * Time.deltaTime);
    }
}
