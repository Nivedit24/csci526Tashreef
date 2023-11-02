using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement playerMovement;
    public float energyDepletionFactor = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan span = DateTime.UtcNow - playerMovement.powerStartTime;
        playerMovement.energyBar.SetHealth((int)(playerMovement.energyLeft - (span.TotalSeconds * energyDepletionFactor)));
        Debug.Log("Energy Left : " + playerMovement.energyBar.slider.value);

        if (playerMovement.energyBar.slider.value <= 0)
        {
            if(playerMovement.currState == State.Hover)
                playerMovement.DismountAirBall();
            if (playerMovement.currState == State.Shielded)
                playerMovement.RemoveEarthShield();
            playerMovement.energyBar.gameObject.SetActive(false);
            playerMovement.ResetUsedCollectables(playerMovement.energyBalls);
        }
    }

}
