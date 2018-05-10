using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBulletColor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Gradient gradient;
    public float fadeTimer = 4.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.color = gradient.Evaluate(Mathf.Abs((Time.time % (fadeTimer * 2f)) / fadeTimer - 1f));
    }
}
