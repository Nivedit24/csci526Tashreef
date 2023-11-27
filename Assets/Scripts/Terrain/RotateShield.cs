using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShield : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject heartEnergy;
    public PlayerMovement playerMovement;
    public int shieldDamage = 2;
    void Start()
    {

    }

    public float speed = 5000f;
    private float direction = 0f;
    public bool startRotate = false;

    void LateUpdate()
    {
        if (startRotate)
        {
            direction = -Input.GetAxis("Horizontal");
            //Keep rotating the object around previous direction
            transform.Rotate(Vector3.forward * speed * Time.deltaTime * direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EarthMonster") || collision.gameObject.CompareTag("Demon") || collision.gameObject.CompareTag("BossMonster"))
        {
            collision.gameObject.GetComponent<EnemyDamage>().TakeDamage(shieldDamage);
            if (collision.gameObject.GetComponent<EnemyDamage>().currHealth <= 0)
            {
                if (collision.gameObject.GetComponent<EnemyMovement>().isFrozen)
                {
                    collision.gameObject.GetComponent<FreezeUnfreezeObject>().UnFreeze();
                }
                if (collision.gameObject.GetComponent<EnemyDamage>().giveHeart)
                {
                    GameObject instatiatedPrefab = Instantiate(heartEnergy, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    playerMovement.heartStore.Add(instatiatedPrefab);
                }
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EarthMonster") || collision.gameObject.CompareTag("Demon") || collision.gameObject.CompareTag("BossMonster"))
        {
            collision.gameObject.GetComponent<EnemyDamage>().TakeDamage(shieldDamage);
            if (collision.gameObject.GetComponent<EnemyDamage>().currHealth <= 0)
            {
                if (collision.gameObject.GetComponent<EnemyMovement>().isFrozen)
                {
                    collision.gameObject.GetComponent<FreezeUnfreezeObject>().UnFreeze();
                }
                if (collision.gameObject.GetComponent<EnemyDamage>().giveHeart)
                    Instantiate(heartEnergy, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                collision.gameObject.SetActive(false);
            }
        }
    }
}
