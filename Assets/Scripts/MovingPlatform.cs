using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 1.2f;
    public float rangeX = 1f;
    private float direction = 1;
    private float startX;

    private void Start()
    {
        startX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (transform.position.x > startX + rangeX)
        {
            direction *= -1;
        }
        else if (transform.position.x < startX)
        {
            direction *= -1;
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime * direction);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
