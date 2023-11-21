using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 2;
    private float direction = 1f;
    private bool isVerticle = true;
    private float startY;
    private float startX;
    public float Range = 1f;
    private float flip = 1f;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        startX = transform.position.x;
        var parentObject = transform.gameObject.GetComponentInParent<Transform>();

        //If parent cloud object is rotated 180 degrees, flip the direction of the arrow
        float angle = parentObject.transform.eulerAngles.z;
        if (angle == 180)
        {
            flip = -1f;
        }
        else if (angle == 90)
        {
            flip = 1f;
            isVerticle = false;
        }
        else if (angle == -90 || angle == 270)
        {
            flip = -1f;
            isVerticle = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move object up and down
        transform.Translate(Vector3.up * Time.deltaTime * speed * direction * flip);
        if (isVerticle)
        {
            if (transform.position.y > startY + Range)
            {
                direction = -1f;
            }
            else if (transform.position.y <= startY - Range)
            {
                direction = 1f;
            }
        }
        else
        {
            if (transform.position.x > startX + Range)
            {
                direction = 1f;
            }
            else if (transform.position.x <= startX - Range)
            {
                direction = -1f;
            }
        }
    }
}
