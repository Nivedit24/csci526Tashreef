using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public float life = 3;
    public Rigidbody2D enemyPrefab;
    public float speed = 20.0f;
    public GameObject enemy;
    public GameObject boss;
    private EnemyMovement enemyMovement;
    void Start()
    {
        if (gameObject.CompareTag("DemonFireball") || gameObject.CompareTag("Boulder"))
        {
            enemyMovement = enemy.GetComponent<EnemyMovement>();
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), enemyMovement.GetComponent<Collider2D>());
            SetInitialVelocity();
        }
        if (gameObject.CompareTag("BossFireball") || gameObject.CompareTag("BossSnowball") || gameObject.CompareTag("BossBoulder"))
        {
            enemyMovement = boss.GetComponent<EnemyMovement>();
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), enemyMovement.GetComponent<Collider2D>());
            SetInitialVelocityBossPrefab();
        }
    }
    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("DemonFireball") || gameObject.CompareTag("lightning") || gameObject.CompareTag("BossFireball") || gameObject.CompareTag("BossSnowball") || gameObject.CompareTag("BossBoulder"))
            Destroy(gameObject);
        else if (gameObject.CompareTag("AcidBlock"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void SetInitialVelocity()
    {
        if (enemyMovement != null)
        {
            float dir = enemyMovement.speed > 0 ? -1 : 1;
            dir = enemyMovement.speed == 0 ? enemyMovement.staticEnemyProjectileDir : dir;
            enemyPrefab.velocity = new Vector2(dir * speed, enemyPrefab.velocity.y);
        }
    }
    void SetInitialVelocityBossPrefab()
    {
        if (enemyMovement != null)
        {
            if (gameObject.transform.eulerAngles.y == 0f)
            {
                enemyPrefab.velocity = new Vector2(1 * speed, enemyPrefab.velocity.y);
            }
            else
                enemyPrefab.velocity = new Vector2(-1 * speed, enemyPrefab.velocity.y);
        }
    }
}
