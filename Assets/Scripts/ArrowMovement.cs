using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 2;
    private float direction = 1f;
    private float startY;
    public float yRange = 1f;
    private float flip = 1f;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        var parentObject = transform.gameObject.GetComponentInParent<Transform>();

        //If parent cloud object is rotated 180 degrees, flip the direction of the arrow

        if (parentObject.transform.eulerAngles.z == 180)
        {
            flip = -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move object up and down
        transform.Translate(Vector3.up * Time.deltaTime * speed * direction * flip);

        //Flip direction after moving yRange
        if (transform.position.y > startY + yRange)
        {
            direction = -1f;
        }
        else if (transform.position.y <= startY - yRange)
        {
            direction = 1f;
        }
    }
}
