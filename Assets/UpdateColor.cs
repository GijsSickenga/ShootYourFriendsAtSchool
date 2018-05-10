using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateColor : MonoBehaviour
{
    private SpriteRenderer parentSpriteRenderer, spriteRenderer;

    void Start()
    {
        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.color = parentSpriteRenderer.color;
    }
}
