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
    public float totalFireballs = 5;
    public float remainingFireballs = 5;
    public PlayerMovement playerMovement;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && shootTime <= 0)
        {
            if (playerMovement.faceRight)
                Instantiate(fireballPrefab, launchPointRight.position, launchPointRight.rotation);
            else
                Instantiate(fireballPrefab, launchPointLeft.position, launchPointLeft.rotation);
            shootTime = 0.25f;
            remainingFireballs -= 1;
        }
        shootTime -= Time.deltaTime;

        if (remainingFireballs <= 0)
        {
            remainingFireballs = 5;
            totalFireballs = 5;
            playerMovement.ResetUsedCollectables(playerMovement.energyBalls);
            ;
            enabled = false;
        }
    }

    public void collectFireballs()
    {
        remainingFireballs = 5;
        totalFireballs = 5;
    }
}
