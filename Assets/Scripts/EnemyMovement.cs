using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    public GameObject projectilePrefab;
    public Transform[] LaunchPoints;

    private bool isFrozen = false;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void OnDisable()
    {
        // Cancel the InvokeRepeating when the GameObject is deactivated
        if (gameObject.tag == "Demon" || gameObject.tag == "EarthMonster")
        {
            CancelInvoke("LaunchProjectiles");
        }
    }

    void OnEnable()
    {
        if (gameObject.tag == "Demon" || gameObject.tag == "EarthMonster")
        {
            InvokeRepeating("LaunchProjectiles", 0f, 3.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Earth Collision detected");
        if (collision.gameObject.CompareTag("Edge") || collision.gameObject.CompareTag("Wall"))
        {
            speed = -speed;
        }

        //Ignore collision with other enemies and spikes
        if (collision.gameObject.CompareTag("Airball") || collision.gameObject.CompareTag("DeathFloor"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if( transform.gameObject.tag == "Demon" && collision.gameObject.tag == "PlayerSnowBall")
        {
            Debug.Log("Demon got hit by snowball");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.gameObject.tag == "Demon" && collision.gameObject.tag == "PlayerSnowBall")
        {
            Debug.Log("Demon got hit by snowball");
            isFrozen = true;
            StartCoroutine(FreezeAndUnfreeze());
            OnDisable();
            Destroy(collision.gameObject);
        }

    }

    void LaunchProjectiles()
    {
        var dirIndex = speed >= 0 ? 0 : 1;
        Quaternion rotation = gameObject.tag == "EarthMonster" ? Quaternion.identity : LaunchPoints[dirIndex].rotation;
        GameObject instantiatedPrefab = Instantiate(projectilePrefab, LaunchPoints[dirIndex].position, rotation);
        instantiatedPrefab.GetComponent<EnemyProjectile>().enemy = this.gameObject;
    }

    IEnumerator FreezeAndUnfreeze()
    {
        // Freeze the demon for 5 seconds
        speed = 0f;
        yield return new WaitForSeconds(5f);

        // Unfreeze the demon and start moving again
        isFrozen = false;
        speed = 10f; // Set speed to its absolute value
        OnEnable();
    }
}
