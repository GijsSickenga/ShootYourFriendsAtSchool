using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class LocalPlayerController : MonoBehaviour {
    //Player movement speed
    public float speed = 5f;

    //offset of the weapon to the middle of the player
    public Vector3 weaponOffset = new Vector3(0.10f, 0.0f, 0);

    //Default weapon
    public GameObject defWeapon;

    //player information (Based on controller number)
    public GameObject playerSprite;
    public TextMesh PlayerID;
    public int playerIndex;
    public PlayerIndex pIndex;

    public GamePadState prevState;
    public GamePadState currentState;

    private Vector2 moveJoy;
    private Vector2 aimJoy;

    //This is for the plyersprite object. Pivot allows the gun rotate the center of the player
    //and sprite is for letting the playersprite object to get a sprite.
    public Transform pivot;
    private SpriteRenderer sprite;

    // This will let the object using animations via animator (For example: switching animation stands)  
    Animator animator;
    public bool inPitfallRoom = false;
    public bool standingOnPlatform = false;
    private bool falling;
    public float fallingTimer;
    private float fallingTimerMax;
    private bool stun;

    private Vector3 defaultScale;
    private Health health;
    private Rigidbody2D body;
    private Vector2 startVelocity;

    public bool isInteracting;
    private bool hasFired = false;

    void Start()
    {
        GiveDefaultWeapon();
        
        //Giving player it's info
        animator = playerSprite.GetComponent<Animator>();
        sprite = playerSprite.GetComponent<SpriteRenderer>();
        PlayerID.text = "P" + (playerIndex + 1);

        fallingTimerMax = fallingTimer;
        stun = false;
        health = GetComponent<Health>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float RightXStick = currentState.ThumbSticks.Right.X;
        float RightYStick = currentState.ThumbSticks.Right.Y;
        float LeftXStick = currentState.ThumbSticks.Left.X;
        float LeftYStick = currentState.ThumbSticks.Left.Y;

        //when the right stick is at a certain position, the gun sprite will be under the player sprite. To give the illusion of depth.
        if (RightYStick > 0.3)
        {
            sprite.sortingOrder = 1;
        }

        else
        {
            sprite.sortingOrder = 0;
        }

        animator.SetFloat("RightXStick", RightXStick);
        animator.SetFloat("RightYStick", RightYStick);
        animator.SetFloat("LeftXStick", LeftXStick);
        animator.SetFloat("LeftYStick", LeftYStick);

        currentState = GamePad.GetState(pIndex, GamePadDeadZone.None);

        if (!currentState.IsConnected)
        {
            //print("Controller not connected");
            return;
        }
        

        if (falling)
        {
            if (fallingTimer > 0)
            {
                fallingTimer -= Time.deltaTime;
                transform.localScale = Easing.OutCubic(fallingTimerMax - fallingTimer, defaultScale, new Vector3(0, 0, defaultScale.z), fallingTimerMax);
                body.velocity = Easing.OutCubic(fallingTimerMax - fallingTimer, startVelocity, Vector2.zero, fallingTimerMax);
            }
            else
            {
                falling = false;
                inPitfallRoom = false;
                standingOnPlatform = false;
                // Reset player size
                transform.localScale = defaultScale;

                health.Damage(health.maxHealth);
            }
        }
        else if(!stun)
        {
            moveJoy.x = currentState.ThumbSticks.Left.X;
            moveJoy.y = currentState.ThumbSticks.Left.Y;

            aimJoy.x = currentState.ThumbSticks.Right.X;
            aimJoy.y = currentState.ThumbSticks.Right.Y;

            if (GameController.roundStart == true)
            {
                if (Mathf.Abs(moveJoy.x) > 0.3 || Mathf.Abs(moveJoy.y) > 0.3)
                    body.velocity = moveJoy * speed;
                else
                {
                    body.velocity = Vector2.zero;
                }

                if (Mathf.Abs(aimJoy.x) > 0.3 || Mathf.Abs(aimJoy.y) > 0.3)
                {
                    float heading = Mathf.Atan2(aimJoy.y, aimJoy.x);
                    pivot.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
                }

                GameObject currentWeapon = GetCurrentWeapon();

                if (currentState.Triggers.Right >= 0.1f && currentWeapon != null)
                {
                    if (currentWeapon.GetComponent<PistolWeapon>() == null)
                    {
                        currentWeapon.GetComponent<WeaponBase>().Fire();
                        hasFired = true;
                    }
                    else // If a pistol is held
                    {
                        // Only fire when the player presses the button
                        if (!hasFired)
                        {
                            currentWeapon.GetComponent<WeaponBase>().Fire();
                            hasFired = true;
                        }
                    }
                }
                else
                {
                    hasFired = false;
                }
            }


            if (prevState.Buttons.RightShoulder == ButtonState.Released && currentState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                GetComponent<MeleeAttack>().TryMelee();
            }

            if(currentState.Buttons.X == ButtonState.Pressed)
            {
                isInteracting = true;
            } else
            {
                isInteracting = false;
            }

            if (inPitfallRoom && !standingOnPlatform && health.currentHealth > 0)
            {
                falling = true;
                fallingTimer = fallingTimerMax;
                defaultScale = transform.localScale;
                body.velocity = body.velocity * 0.5f;
                startVelocity = body.velocity;
                   
            }

            inPitfallRoom = false;
            standingOnPlatform = false;
        }

        prevState = currentState;
    }

    public GameObject GetCurrentWeapon()
    {
        foreach (Transform child in pivot.transform)
        {
            if (child.CompareTag("Weapon"))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private bool HasDefaultWeapon()
    {
        if (GetCurrentWeapon().Equals(defWeapon))
            return true;

        return false;
    }

    public void GiveDefaultWeapon()
    {
        GiveWeapon(defWeapon);
    }

    public WeaponBase GiveWeapon(GameObject weapon)
    {
        GameObject gun = GetCurrentWeapon();
        if (gun != null)
        {
            Destroy(gun);
        }

        GameObject theWeapon = Instantiate(weapon, transform.position, transform.rotation) as GameObject;

        theWeapon.transform.parent = this.pivot;
        theWeapon.transform.localRotation = Quaternion.identity;
        theWeapon.transform.localPosition = weaponOffset;
        theWeapon.transform.localScale = new Vector2(1f, 1f);

        return theWeapon.GetComponent<WeaponBase>();
    }



    public IEnumerator StunForSeconds(float sec)
    {
        stun = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(sec);
        stun = false;
    }

    public IEnumerator StunForSeconds(float sec, float drag)
    {
        stun = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().drag = drag;
        yield return new WaitForSeconds(sec);
        stun = false;
        GetComponent<Rigidbody2D>().drag = 0.0f;
    }
}
