using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D volcanoBall;
    public float speed = 5.0f;
    void Start()
    {
        volcanoBall = GetComponent<Rigidbody2D>();
        setVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setVelocity()
    {
        volcanoBall.velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
