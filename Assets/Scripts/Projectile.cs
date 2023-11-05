using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D projectileBody;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (gameObject.tag)
        {
            case "PlayerFireball":
                if (collision.gameObject.tag == "Demon" || collision.gameObject.tag == "EarthMonster")
                {
                    collision.gameObject.GetComponent<EnemyDamage>().TakeDamage(50);
                    if (collision.gameObject.GetComponent<EnemyDamage>().currHealth <= 0)
                    {
                        collision.gameObject.SetActive(false);
                    }
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
                break;
            case "PlayerSnowBall":
                string collisionTag = collision.gameObject.tag;
                if (collisionTag != "AcidDrop" && collisionTag != "IceMonster" && collisionTag != "Demon" && collisionTag != "Untagged")
                {
                    Destroy(transform.gameObject);
                }
                break;
        }

    }
    void SetInitialVelocity()
    {
        if (playerMovement != null)
        {
            float dir = playerMovement.faceRight == false ? -1 : 1;
            projectileBody.velocity = new Vector2(dir * speed, projectileBody.velocity.y);
        }
    }
}
