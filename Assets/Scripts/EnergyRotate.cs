using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRotate : MonoBehaviour
{

    public float speed = 200f;

    // Start is called before the first frame update
    void Start()
    {

    }


    void LateUpdate()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
