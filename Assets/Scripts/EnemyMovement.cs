using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    public GameObject projectilePrefab;
    public Transform[] LaunchPoints;
    void Start()
    {
        if (gameObject.tag == "Demon")
        {
            InvokeRepeating("LaunchProjectiles", 0f, 5.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Edge"))
        {
            Debug.Log("Edge");
            speed = -speed;
        }

        //Ignore collision with other enemies and spikes
        if (collision.gameObject.CompareTag("Airball") || collision.gameObject.CompareTag("DeathFloor"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void LaunchProjectiles()
    {
        if (speed >= 0)
        {
            Instantiate(projectilePrefab, LaunchPoints[0].position, LaunchPoints[0].rotation);
        }
        else
        {
            Instantiate(projectilePrefab, LaunchPoints[1].position, LaunchPoints[1].rotation);
        }
        
    }
}
