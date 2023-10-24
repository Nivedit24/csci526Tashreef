using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D fireBall;
    public float speed = 50.0f;
    public float projectileCount = 5.0f;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        SetInitialVelocity();
    }

    void Update()
    {
        projectileCount -= Time.deltaTime;
        if (projectileCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Demon")
        {
            collision.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void SetInitialVelocity()
    {
        if (playerMovement != null)
        {
            float dir = playerMovement.faceRight == false ? -1 : 1;
            fireBall.velocity = new Vector2(dir * speed, fireBall.velocity.y);
        }
    }
}
