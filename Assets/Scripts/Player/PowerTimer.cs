using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement playerMovement;
    public float energyDepletionFactor = 10f;
    public float energyReplenishFactor = 5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.currState == State.Hover || playerMovement.currState == State.Shielded)
        {
            DepletePower();
        }
        else
        {
            ReplenishPower();
        }
    }

    private void DepletePower()
    {
        TimeSpan span = DateTime.UtcNow - playerMovement.powerStartTime;
        playerMovement.energyBar.SetHealth((int)(playerMovement.energyLeft - (span.TotalSeconds * energyDepletionFactor)));

        if (playerMovement.energyBar.slider.value <= 0)
        {
            if (playerMovement.currState == State.Hover)
                playerMovement.DismountAirBall();
            if (playerMovement.currState == State.Shielded)
                playerMovement.RemoveEarthShield();

            playerMovement.SetEnergyLevel(0);
            playerMovement.ResetUsedCollectables(playerMovement.energyBalls);
        }
    }
    private void ReplenishPower()
    {
        TimeSpan span = DateTime.UtcNow - playerMovement.powerEndTime;
        playerMovement.energyBar.SetHealth((int)(playerMovement.energyLeft + (span.TotalSeconds * energyReplenishFactor)));
    }
}
