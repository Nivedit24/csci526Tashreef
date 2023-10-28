using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public float curScale = 0.25f;
    private bool check = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curScale = check ? curScale + 0.001f : curScale - 0.001f;
        if (curScale >= 0.35 || curScale<=0.25)
            check = !check;

        transform.localScale = new Vector3(curScale, curScale, 1.0f);
    }
}
