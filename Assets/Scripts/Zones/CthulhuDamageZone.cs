using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CthulhuDamageZone : MonoBehaviour
{
    public ParticleSystemStarter psStarter;
    public ParticleSystemStarter psLaser;

    public Vector2 inactiveTimeRange = new Vector2(3, 13);
    public Vector2 warningTimeRange = new Vector2(3, 3);
    public Vector2 activeTimeRange = new Vector2(1, 1);
    private float inactiveTimer, warningTimer, activeTimer;
    public Vector2 damageRange = new Vector2(100, 100);
    public Color inactiveColor = new Color(0f, 1f, 0f, 0.1f);
    public Color warningColor = new Color(1f, 0.75f, 0f, 0.75f);
    public Color activeColor = new Color(1f, 0f, 0f, 0.75f);
    public bool rainbowMode;
    public Texture2D warningTexture;
    public Vector2 warningTextureSize;
    private bool showWarningTexture = false;
    private float activeHue = 0;

    private List<GameObject> players = new List<GameObject>();

    private ZoneVisualizer zoneVisualizer;

    private enum State { Inactive, Warning, Active };
    private State state;

    void Start()
    {
        zoneVisualizer = GetComponent<ZoneVisualizer>();

        inactiveTimer = Random.Range(inactiveTimeRange.x, inactiveTimeRange.y);
        warningTimer = Random.Range(warningTimeRange.x, warningTimeRange.y);
        activeTimer = Random.Range(activeTimeRange.x, activeTimeRange.y);
    }

    void Update()
    {
        switch (state)
        {
            case State.Inactive:
                inactiveTimer -= Time.deltaTime;

                if (inactiveTimer <= 0)
                {
                    inactiveTimer = Random.Range(inactiveTimeRange.x, inactiveTimeRange.y);
                    state = State.Warning;
                    //zoneVisualizer.ChangeColor(warningColor);
                    //showWarningTexture = true;
                    if(psStarter != null)
                        psStarter.StartParticleSystems();
                }
                break;

            case State.Warning:
                warningTimer -= Time.deltaTime;
                
                if (warningTimer <= 0)
                {
                    warningTimer = Random.Range(warningTimeRange.x, warningTimeRange.y);
                    state = State.Active;
                    //zoneVisualizer.ChangeColor(activeColor);
                    //showWarningTexture = false;
                    if (psLaser != null)
                        psLaser.StartParticleSystems();
                }
                break;

            case State.Active:
                activeTimer -= Time.deltaTime;

                if (rainbowMode)
                {
                    activeHue += 0.2f;
                    if (activeHue > 1) { activeHue %= 1; }
                    //zoneVisualizer.ChangeColor(Color.HSVToRGB(activeHue, 1, 1));
                }

                if (activeTimer <= 0)
                {
                    activeTimer = Random.Range(activeTimeRange.x, activeTimeRange.y);
                    state = State.Inactive;
                    zoneVisualizer.ChangeColor(inactiveColor);
                }

                foreach (GameObject player in players)
                {
                    Vector3 playerPosition = player.transform.position;
                    // Make sure the player is standing at least halfway inside the area
                    if (playerPosition.x >= transform.position.x &&
                        playerPosition.x <= transform.position.x + transform.lossyScale.x &&
                        playerPosition.y <= transform.position.y &&
                        playerPosition.y >= transform.position.y - transform.lossyScale.y)
                    {
                        Health health = player.GetComponent<Health>();
                        health.Damage(Random.Range(damageRange.x, damageRange.y) / 100 * health.maxHealth);
                    }
                }
                break;
        }
    }

    //void OnGUI()
    //{
    //    if (showWarningTexture)
    //    {
    //        GUI.DrawTexture(new Rect(Camera.main.WorldToScreenPoint(transform.position + transform.localScale / 2).x - warningTextureSize.x / 2,
    //                                 -Camera.main.WorldToScreenPoint(transform.position - transform.localScale / 2).y + Screen.height - warningTextureSize.y / 2,
    //                                 warningTextureSize.x,
    //                                 warningTextureSize.y),
    //                        warningTexture);
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            players.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            players.Remove(other.gameObject);
        }
    }
}
