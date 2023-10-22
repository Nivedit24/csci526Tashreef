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
        volcanoBall.velocity = transform.right * Random.Range(15.0f, 20.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("VolcanoBall"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
