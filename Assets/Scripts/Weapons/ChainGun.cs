using UnityEngine;
using System.Collections;

public class ChainGun : WeaponBase
{
    public float startFireRate, endPitchMin, endPitchMax, warmUpTimer;
    private float endFireRate, startPitchMin, startPitchMax;
    private LocalPlayerController currentPlayerController;

    public override void Start()
    {
        base.Start();

        endFireRate = fireRate;
        fireRate = startFireRate;

        startPitchMin = pitchMin;
        startPitchMax = pitchMax;

        currentPlayerController = currentPlayer.GetComponent<LocalPlayerController>();
    }

    public override void Update()
    {
        if (currentPlayerController.currentState.Triggers.Right >= 0.1f)
        {
            if (fireRate > endFireRate)
            {
                fireRate += ((endFireRate - startFireRate) / warmUpTimer) * Time.deltaTime;
            }
            else
            {
                fireRate = endFireRate;
            }

            if (pitchMin < endPitchMin)
            {
                pitchMin += ((endPitchMin - startPitchMin) / warmUpTimer) * Time.deltaTime;
            }
            else
            {
                pitchMin = endPitchMin;
            }

            if (pitchMax < endPitchMax)
            {
                pitchMax += ((endPitchMax - startPitchMax) / warmUpTimer) * Time.deltaTime;
            }
            else
            {
                pitchMax = endPitchMax;
            }
        }
        else
        {
            if (fireRate < startFireRate)
            {
                fireRate += ((startFireRate - endFireRate) / warmUpTimer) * (Time.deltaTime * 2f);
            }

            if (fireRate > startFireRate)
            {
                fireRate = startFireRate;
            }

            if (pitchMin > startPitchMin)
            {
                pitchMin += ((startPitchMin - endPitchMin) / warmUpTimer) * (Time.deltaTime * 2f);
            }

            if (pitchMin < startPitchMin)
            {
                pitchMin = startPitchMin;
            }

            if (pitchMax > startFireRate)
            {
                pitchMax += ((startPitchMax - endPitchMax) / warmUpTimer) * (Time.deltaTime * 2f);
            }

            if (pitchMax < startPitchMax)
            {
                pitchMax = startPitchMax;
            }
        }

        base.Update();
    }
}
