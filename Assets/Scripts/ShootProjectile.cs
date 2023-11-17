using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firePrefab;
    public GameObject icePrefab;

    public Transform launchPointRight;
    public Transform launchPointLeft;
    public float shootTime = 0.25f;
    public PlayerMovement playerMovement;

    public float shootEnergy = 20f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject projectile = playerMovement.currPower == Power.Fire ? firePrefab : icePrefab;

        if (playerMovement.energyLeft >= 0 && Input.GetKeyDown(KeyCode.Space) && shootTime <= 0)
        {
            if (playerMovement.faceRight)
                Instantiate(projectile, launchPointRight.position, launchPointRight.rotation);
            else
                Instantiate(projectile, launchPointLeft.position, launchPointLeft.rotation);
            shootTime = 0.25f;
            playerMovement.energyBar.slider.value -= shootEnergy;
            playerMovement.energyLeft = playerMovement.energyBar.slider.value;
            playerMovement.powerEndTime = System.DateTime.UtcNow;
            if (playerMovement.energyBar.slider.value <= 0)
            {
                playerMovement.SetEnergyLevel(0);
                playerMovement.ResetUsedCollectables(playerMovement.energyBalls);
            }
        }
        shootTime -= Time.deltaTime;
    }
}
