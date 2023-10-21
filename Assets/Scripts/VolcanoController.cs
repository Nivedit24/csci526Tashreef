using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoController : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform[] LaunchPoints;  
    public float launchInterval = 5f;   // Time interval between each launch
    public float projectileSpeed = 5f;  

    void Start()
    {
        InvokeRepeating("LaunchProjectiles", 0f, launchInterval);
    }

    void LaunchProjectiles()
    {    
        for(int i=0;i<LaunchPoints.Length;i++)
            Instantiate(projectilePrefab, LaunchPoints[i].position, LaunchPoints[i].rotation);
    }
}
