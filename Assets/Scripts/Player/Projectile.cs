using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D projectileBody;
    public float speed = 100.0f;
    public float projectileCount = 5.0f;
    private PlayerMovement playerMovement;
    public GameObject heartEnergy;
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
                playerMovement.fireShotCount++;
                if (playerMovement.lastPowerUsed != Power.Fire.ToString())
                {
                    playerMovement.callPowerPairAnalytics(playerMovement.lastPowerUsed, Power.Fire.ToString());
                }
                playerMovement.lastPowerUsed = Power.Fire.ToString();

                if (collision.gameObject.tag == "Demon" || collision.gameObject.tag == "EarthMonster" || collision.gameObject.tag == "BossMonster")
                {
                    collision.gameObject.GetComponent<EnemyDamage>().TakeDamage(50);
                    if (collision.gameObject.GetComponent<EnemyDamage>().currHealth <= 0)
                    {
                        if (collision.gameObject.GetComponent<EnemyMovement>().isFrozen)
                        {
                            collision.gameObject.GetComponent<FreezeUnfreezeObject>().UnFreeze();
                        }
                        if (collision.gameObject.GetComponent<EnemyDamage>().giveHeart)
                        {
                            GameObject instantiatedPrefab = Instantiate(heartEnergy, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                            playerMovement.heartStore.Add(instantiatedPrefab);
                        }
                        collision.gameObject.SetActive(false);
                    }
                    Destroy(gameObject);
                }
                else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("BreakWall"))
                {
                    Destroy(gameObject);
                }
                else
                {
                    Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
                break;
            case "PlayerSnowBall":
                playerMovement.iceShotCount++;
                if (playerMovement.lastPowerUsed != Power.Water.ToString())
                {
                    playerMovement.callPowerPairAnalytics(playerMovement.lastPowerUsed, Power.Water.ToString());
                }
                playerMovement.lastPowerUsed = Power.Water.ToString();

                string collisionTag = collision.gameObject.tag;
                if (collisionTag == "Cloud" || collisionTag == "LayerRestorer" || collisionTag == "Sand" || collisionTag == "VolcanoBall" || collisionTag == "CheckPoint" || collisionTag == "EnergyBall")
                {
                    Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
                else if (collisionTag != "AcidDrop" && collisionTag != "IceMonster" && collisionTag != "Demon" && collisionTag != "Untagged")
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
