using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = transform.parent.GetComponent<SpriteRenderer>().color;
    }
}
