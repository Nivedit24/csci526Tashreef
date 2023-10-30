using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement playerMovement;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan span = DateTime.UtcNow - playerMovement.startTime;
        playerMovement.energyBar.SetHealth((int)(playerMovement.energyLeft - (span.TotalSeconds * 10)));
        Debug.Log("Energy Left : " + playerMovement.energyBar.slider.value);

        if (playerMovement.energyBar.slider.value <= 0)
        {
            playerMovement.DismountAirBall();
            playerMovement.energyBar.gameObject.SetActive(false);
            playerMovement.ResetUsedCollectables(playerMovement.energyBalls);
        }
    }

}
