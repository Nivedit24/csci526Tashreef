using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMovement : MonoBehaviour
{
    public GameObject switches;
    public GameObject platform;
    public bool activated;
    public float startPositionY;
    public int direction = 1;
    public float speed = 5.0f;
    public float rangeY = 10.0f;
    void Start()
    {
       startPositionY = platform.transform.position.y;
       switches.GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activated)
        {
            if (platform.transform.position.y > startPositionY + rangeY)
            {
                direction = -1;
            }
            else if (platform.transform.position.y < startPositionY)
            {
                direction = 1;
            }
            platform.transform.Translate(Vector3.up * speed * Time.deltaTime * direction);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PlayerFireball")
        {
            if (!activated)
            {
                activated = true;
                switches.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                activated = false;
                switches.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
}
