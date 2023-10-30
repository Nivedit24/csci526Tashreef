using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public float life = 3;
    public Rigidbody2D enemyPrefab;
    public float speed = 20.0f;
    public GameObject enemy;
    private EnemyMovement enemyMovement;
    void Start()
    {
        if (gameObject.CompareTag("DemonFireball") || gameObject.CompareTag("Boulder"))
        {
            enemyMovement = enemy.GetComponent<EnemyMovement>();
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
        if (gameObject.CompareTag("DemonFireball"))
            Destroy(gameObject);
    }

    void SetInitialVelocity()
    {
        if (enemyMovement != null)
        {
            float dir = enemyMovement.speed > 0 ? -1 : 1;
            enemyPrefab.velocity = new Vector2(dir * speed, enemyPrefab.velocity.y);
        }
    }
}
