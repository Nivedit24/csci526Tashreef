using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fireballPrefab;
    public Transform launchPointRight;
    public Transform launchPointLeft;
    public float shootTime = 0.25f;
    public PlayerMovement playerMovement;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.energyBar.slider.value > 0 && Input.GetKeyDown(KeyCode.Space) && shootTime <= 0)
        {
            if (playerMovement.faceRight)
                Instantiate(fireballPrefab, launchPointRight.position, launchPointRight.rotation);
            else
                Instantiate(fireballPrefab, launchPointLeft.position, launchPointLeft.rotation);
            shootTime = 0.25f;
            playerMovement.energyBar.slider.value -= 10;
            playerMovement.energyLeft = playerMovement.energyBar.slider.value;
        }
        shootTime -= Time.deltaTime;

        if (playerMovement.energyBar.slider.value <= 0)
        {
            playerMovement.SetEnergyLevel(0);
        }
    }
}
