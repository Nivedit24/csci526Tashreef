using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour
{
  
    public float life = 3;
    public Rigidbody2D fireBall;
    public float speed = 20.0f;
    private EnemyMovement enemyMovement;
    void Start()
    {
        if (gameObject.CompareTag("DemonFireball"))
        {
            enemyMovement = GameObject.FindGameObjectWithTag("Demon").GetComponent<EnemyMovement>();
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), enemyMovement.GetComponent<Collider2D>());
            SetInitialVelocity();
        }
    }
    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    void SetInitialVelocity()
    {
        if (enemyMovement != null)
        {
            if (enemyMovement.speed<0)
            {
                fireBall.velocity = new Vector2(speed, fireBall.velocity.y);
            }
            else
            {
                fireBall.velocity = new Vector2(-speed, fireBall.velocity.y);
            }
        }
    }
}
