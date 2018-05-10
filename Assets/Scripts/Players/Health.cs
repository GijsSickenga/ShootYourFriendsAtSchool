using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public RectTransform healthBar;
    GameObject spawnManagerObj;

    public bool invulnerable, invulnerableToTraps;
    private float invulnerableTimer, invulnerableToTrapsTimer = 0f;
    private float invulnerableTimerMax = 1.0f;

    //index is needed for scoring
    public PlayerIndex playerIndex;

    //when a bullet of object last hit the player. For adding or substracting scores.
    public int lastHit;
    public float lastHitTime = 3.0f;
    private float lastHitTimer;

    public bool vibrateController = true;
    public float vibrationTime = 0.2f;
    public float maxVibrateAmount = 0.7f;

    void Start()
    {
        currentHealth = maxHealth;
        playerIndex = GetComponentInParent<LocalPlayerController>().pIndex;
        spawnManagerObj = GameObject.FindWithTag("SpawnManager");

        invulnerable = false;
        invulnerableTimer = 0.0f;
    }

    public void Damage(float amount)
    {
        if (invulnerable)
            return;

        currentHealth -= amount;

        if (amount > maxHealth)
            amount = maxHealth;

        if (vibrateController)
        {
            Vibration.VibrateForSeconds(vibrationTime, amount / maxHealth * maxVibrateAmount, playerIndex);
        }

        CheckIfDead();

        UpdateHealthbar(currentHealth);
    }
    public void Damage(float amount, float invulnerableToTrapsTimer)
    {
        Damage(amount);
        this.invulnerableToTrapsTimer = invulnerableToTrapsTimer;
        invulnerableToTraps = true;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        CheckMaxHealth();

        UpdateHealthbar(currentHealth);
    }

    public void SetHealth(float amount)
    {
        currentHealth = amount;

        CheckMaxHealth();

        CheckIfDead();

        UpdateHealthbar(currentHealth);
    }

    public void UpdateHealthbar(float health)
    {
        healthBar.sizeDelta = new Vector2(health / 2, healthBar.sizeDelta.y);
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Score((int)playerIndex, lastHit);
            Respawn();
            invulnerable = true;
            invulnerableTimer = invulnerableTimerMax;
        }
    }

    private void Respawn()
    {
        spawnManagerObj.GetComponent<SpawnManager>().Respawn(this.gameObject);
    }

    private void CheckMaxHealth()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void FixedUpdate()
    {
        if (invulnerableTimer > 0)
        {
            invulnerableTimer -= Time.fixedDeltaTime;
        }
        else
        {
            invulnerable = false;
        }

        if (invulnerableToTrapsTimer > 0)
        {
            invulnerableToTrapsTimer -= Time.fixedDeltaTime;
        }
        else
        {
            invulnerableToTraps = false;
        }

        if(lastHitTimer > 0)
        {
            lastHitTimer -= Time.fixedDeltaTime;
        }
        else if(lastHit != -1)
        {
            lastHit = -1;
        }
    }

    //this will add the score to the scoreboard
    void Score(int playerIndex, int lastHit)
    {
        switch (playerIndex)
        {
            case 0: GameController.p1Deaths++; break;
            case 1: GameController.p2Deaths++; break;
            case 2: GameController.p3Deaths++; break;
            case 3: GameController.p4Deaths++; break;
        }

        switch (lastHit)
        {
            case -1: break;
            case 0: GameController.p1Kills++; break;
            case 1: GameController.p2Kills++; break;
            case 2: GameController.p3Kills++; break;
            case 3: GameController.p4Kills++; break;
        }
    }

    public IEnumerator SwitchColorForSeconds(Color color, float seconds)
    {
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Color normalColor = ColorTracker.colors[(int)playerIndex + 1];
        spriteRenderer.color = color;
        yield return new WaitForSeconds(seconds);
        spriteRenderer.color = normalColor;
    }

    public void ResetLastHitTimer()
    {
        lastHitTimer = lastHitTime;
    }
}