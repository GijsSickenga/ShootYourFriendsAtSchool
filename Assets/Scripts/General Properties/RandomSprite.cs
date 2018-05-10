using UnityEngine;
using System.Collections;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
