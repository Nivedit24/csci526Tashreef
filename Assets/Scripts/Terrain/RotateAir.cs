using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAir : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public float speed = 5000f;
    private float direction = 0f;
    public bool startRotate = false;

    void LateUpdate()
    {
        if (startRotate)
        {
            direction = Input.GetAxis("Horizontal");
            //Keep rotating the object around previous direction
            transform.Rotate(Vector3.up * speed * Time.deltaTime * direction);
        }
    }
}
