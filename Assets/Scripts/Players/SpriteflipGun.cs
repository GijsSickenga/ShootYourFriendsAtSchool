using UnityEngine;
using System.Collections;

public class SpriteflipGun : MonoBehaviour
{
    SpriteRenderer sprite, glowSprite;
    public GameObject glow; 
    bool isFlipped, isRange;

    // Use this for initialization
    void Start()
    {
        isFlipped = false;
        isRange = false;
        sprite = GetComponent<SpriteRenderer>();
        glowSprite = glow.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame 
    void Update()
    {
        if (transform.parent.localRotation.eulerAngles.z > 90 && transform.parent.localRotation.eulerAngles.z < 270)
        {
            isRange = true;
            if (isFlipped == false)
            {
                sprite.flipY = true;
                glowSprite.flipY = true;

                isFlipped = true;
            }
        }

        else
        {
            sprite.flipY = false;
            glowSprite.flipY = false;

            isFlipped = false;
        }

     
    }
}