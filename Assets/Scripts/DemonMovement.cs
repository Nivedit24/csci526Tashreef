using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonMovement : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 5.0f;
    private int destination  = 0;
    // Update is called once per frame
    void Update()
    {
        if (destination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
            if(Vector2.Distance(transform.position, startPoint.position) < .3f)
            {
                destination = 1;
            }
        }
        if (destination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, endPoint.position) < .3f)
            {
                destination = 0;
            }
        }
    }
}
