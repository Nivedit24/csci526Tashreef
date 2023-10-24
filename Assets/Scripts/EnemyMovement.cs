using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    public GameObject projectilePrefab;
    public Transform[] LaunchPoints;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void OnDisable()
    {
        // Cancel the InvokeRepeating when the GameObject is deactivated
        if (gameObject.tag == "Demon")
        {
            CancelInvoke("LaunchProjectiles");
        }
    }

    void OnEnable()
    {
        if (gameObject.tag == "Demon")
        {
            InvokeRepeating("LaunchProjectiles", 0f, 3.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Edge"))
        {
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
        var dirIndex = speed >= 0 ? 0 : 1;
        Instantiate(projectilePrefab, LaunchPoints[dirIndex].position, LaunchPoints[dirIndex].rotation);
    }
}
