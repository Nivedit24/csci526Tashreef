using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public float minScale = 0.25f;
    public float maxScale = 0.35f;
    private float pulseTimer = 0.0f;
    private bool check = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pulseTimer += Time.deltaTime;
        if (pulseTimer >= 3)
        {
            if(check)
                transform.localScale = new Vector3(maxScale, maxScale, 1.0f);
            else
                transform.localScale = new Vector3(minScale, minScale, 1.0f);
            pulseTimer = 0;
            check = !check;
        }
    }
}
