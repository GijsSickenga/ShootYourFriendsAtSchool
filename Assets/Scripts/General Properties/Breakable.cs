using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour
{
    public bool randomHealth = false;
    public bool randomRegen = false;
    public bool showHealthbar = false;

    public Color healthBarColor = Color.green;

    public Vector2 healthRange;
    public Vector2 regenTimerRange;

    // Health bar
    public Vector2 healthBarSize;
    public Vector2 healthBarOffset;
    public int healthBarBorderSize;

    private int health, maxHealth;

    private float regenTimer;

    private bool canRegenerate = false;
    private bool regenerating = false;

    private Collider2D myCollider;

    private Rect healthBarRect;

    // Temporary variable for saving object color to reset to when object regenerates
    // Remove this when animations for being destroyed/deactivated and regenerated/reactivated are in place
    private Color previousColor;
    
    private Texture2D texture;
    private GUIStyle style;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();

        if (regenTimerRange.y > 0)
        {
            canRegenerate = true;
        }

        Reset();

        UpdateHealthBar();

        // Generate healthbar texture
        texture = new Texture2D(1, 1);
        style = new GUIStyle();
    }

    void Update()
    {
        if (regenerating)
        {
            regenTimer -= Time.deltaTime;

            if (regenTimer <= 0)
            {
                // Reenable collision box if there is one present in the object this script is attached to
                if (myCollider != null)
                {
                    myCollider.enabled = true;
                }

                // Reset color
                GetComponent<SpriteRenderer>().color = previousColor;

                // Reactivate children
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }

                Reset();
                regenerating = false;
            }
        }
    }

    // Resets the health and regenTimer of this breakable object
    void Reset()
    {
        Health = (int)Mathf.Round(Random.Range(healthRange.x, healthRange.y));
        maxHealth = Health;

        regenTimer = Random.Range(regenTimerRange.x, regenTimerRange.y);
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;
        
        if (Health <= 0)
        {
            if (canRegenerate)
            {
                regenerating = true;

                // Disable collision box when the object is regenerating
                if (myCollider != null)
                {
                    myCollider.enabled = false;
                }

                // Temporary visual when object breaks
                previousColor = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 0.1f);

                // Deactivate children
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }

                // TODO: Clear visual effect when object is destroyed (change sprite/color, animation, generate debris etc.)
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        // Use Damage() to reduce this object's health from outside this script
        private set
        {
            health = value;

            if (showHealthbar)
            {
                UpdateHealthBar();
            }
        }
    }

    void UpdateHealthBar()
    {
        int healthBarLength = (int)(healthBarSize.x / maxHealth * Health);
        Mathf.Clamp(healthBarLength, 0, healthBarSize.x);

        healthBarRect = new Rect(Camera.main.WorldToScreenPoint(transform.position).x - (healthBarSize.x / 2 + healthBarOffset.x) * 8 / Camera.main.orthographicSize,
                                 -Camera.main.WorldToScreenPoint(transform.position - transform.localScale / 2).y + Screen.height - (healthBarOffset.y) * 8 / Camera.main.orthographicSize,
                                 healthBarLength * 8 / Camera.main.orthographicSize,
                                 healthBarSize.y * 8 / Camera.main.orthographicSize);
    }

    void OnGUI()
    {
        if (showHealthbar)
        {
            if (Health > 0)
            {
                // Background
                GUIDrawRect(new Rect(healthBarRect.x - healthBarBorderSize * 8 / Camera.main.orthographicSize,
                                     healthBarRect.y - healthBarBorderSize * 8 / Camera.main.orthographicSize,
                                     (healthBarSize.x + 2 * healthBarBorderSize) * 8 / Camera.main.orthographicSize,
                                     (healthBarSize.y + 2 * healthBarBorderSize) * 8 / Camera.main.orthographicSize),
                            new Color(healthBarColor.r / 4,
                                      healthBarColor.g / 4,
                                      healthBarColor.b / 4,
                                      1));

                // Health slider
                GUIDrawRect(healthBarRect, healthBarColor);
            }
        }
    }

    // Draw a colored rectangle on the screen
    // Only call from OnGUI()
    void GUIDrawRect(Rect myRect, Color color)
    {
        // Color the texture
        texture.SetPixel(0, 0, color);
        texture.Apply();
        style.normal.background = texture;

        // Draw the rectangle
        GUI.Box(myRect, GUIContent.none, style);
    }
}
