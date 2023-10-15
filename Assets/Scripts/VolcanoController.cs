using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoController : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign the projectile prefab in the Inspector
    public Transform leftLaunchPoint;   // Left launch point
    public Transform rightLaunchPoint;  // Right launch point
    public float launchInterval = 5f;   // Time interval between each launch
    public int projectilesPerLaunch = 2; // Number of projectiles to launch in each direction
    public float projectileSpeed = 5f;   // Speed of the projectiles

    void Start()
    {
        InvokeRepeating("LaunchProjectiles", 0f, launchInterval);
    }

    void LaunchProjectiles()
    {    
        Instantiate(projectilePrefab, leftLaunchPoint.position, leftLaunchPoint.rotation);
        Instantiate(projectilePrefab, rightLaunchPoint.position, rightLaunchPoint.rotation);
    }
}
