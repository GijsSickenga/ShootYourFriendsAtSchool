using UnityEngine;
using System.Collections;

public class CthulhuLaserEye : MonoBehaviour
{
    public enum States { Submerged, Emerge, Open, ShowPupil, FireLaser, Disappear, Reset, Idle };
    private States state = States.Reset;
    private States prevState;
    
    // Time to wait before going to the next state
    private float waitTimer;

    // Default wait times at the end of each state
    public float emergeWaitTimer = 0.5f;
    public float openWaitTimer = 0.5f;
    public float showPupilWaitTimer = 0.5f;
    public float fireLaserWaitTimer = 0.5f;

    // Default wait time between shots
    public float sleepTimerMin = 15f, sleepTimerMax = 25f;

    // Subparts of the eye
    public GameObject lid, pupil, sclera;

    // Sprite animation managers for all eye subparts
    private SpriteAnimationManager lidAnimations, pupilAnimations, scleraAnimations;

    void Start()
    {
        lidAnimations = lid.GetComponent<SpriteAnimationManager>();
        pupilAnimations = pupil.GetComponent<SpriteAnimationManager>();
        scleraAnimations = sclera.GetComponent<SpriteAnimationManager>();

        lid.SetActive(false);
        pupil.SetActive(false);
        sclera.SetActive(false);
    }

    void Update()
    {
        switch (state)
        {
            // Eye is invisible and waiting to be activated
            case States.Submerged:
                waitTimer -= Time.deltaTime;

                if (waitTimer <= 0)
                {
                    Emerge();
                }
                break;

            // Eye sprite becomes visible
            case States.Emerge:

                if (lidAnimations.HasActiveAnimationFinished)
                {
                    ChangeState(States.Idle);
                    waitTimer = emergeWaitTimer;
                }
                break;

            // Eyelid opens
            case States.Open:

                if (lidAnimations.HasActiveAnimationFinished)
                {
                    ChangeState(States.Idle);
                    waitTimer = openWaitTimer;
                }
                break;

            // Pupil appears on eye and starts looking around
            case States.ShowPupil:

                if (pupilAnimations.HasActiveAnimationFinished)
                {
                    ChangeState(States.Idle);
                    waitTimer = showPupilWaitTimer;
                }
                break;

            // Laser is fired in front of eye, killing all players that touch it
            case States.FireLaser:

                ChangeState(States.Idle);
                waitTimer = fireLaserWaitTimer;
                break;

            // Eye sprite becomes invisible again after closing and retreating into pit
            case States.Disappear:

                Reset();
                break;

            // Values are reset so that the eye may be activated again
            case States.Reset:

                ChangeState(States.Submerged);
                waitTimer = Random.Range(sleepTimerMin, sleepTimerMax);
                break;

            // Eye pauses for a while between certain actions
            case States.Idle:
                waitTimer -= Time.deltaTime;

                if (waitTimer <= 0)
                {
                    switch (prevState)
                    {
                        case States.Emerge:
                            Open();
                            break;

                        case States.Open:
                            ShowPupil();
                            break;

                        case States.ShowPupil:
                            FireLaser();
                            break;

                        case States.FireLaser:
                            Disappear();
                            break;

                        default:
                            Reset();
                            break;
                    }
                }
                break;
        }
    }

    public void ChangeState(States newState)
    {
        prevState = state;
        state = newState;
    }

    void Emerge()
    {
        ChangeState(States.Emerge);
        lid.SetActive(true);
        lidAnimations.Play("Emerge");
    }

    void Open()
    {
        ChangeState(States.Open);
        lidAnimations.Play("Open");
        sclera.SetActive(true);
        scleraAnimations.Play("Open");
    }

    void ShowPupil()
    {
        ChangeState(States.ShowPupil);
        pupil.SetActive(true);
        pupilAnimations.Play("ShowPupil");
    }

    void FireLaser()
    {
        ChangeState(States.FireLaser);
    }

    void Disappear()
    {
        ChangeState(States.Disappear);
    }

    void Reset()
    {
        ChangeState(States.Reset);
        lid.SetActive(false);
        pupil.SetActive(false);
        sclera.SetActive(false);
    }
}