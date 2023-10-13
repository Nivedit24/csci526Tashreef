using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackLighningCloud : MonoBehaviour
{
    public Transform lightningSpawnPoint;
    public GameObject lighningPrefab;
    public float lightningSpeed;
    private float lightningTimer = 0f;
    public float spawnInterval;
    private bool movingRight = false;
    public float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        lightningTimer += Time.deltaTime;

        if (lightningTimer >= spawnInterval)
        {
            var bullet = Instantiate(lighningPrefab, lightningSpawnPoint.position, lightningSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = (-1) * lightningSpawnPoint.up * lightningSpeed;
            lightningTimer = 0f;
        }

        Vector2 currentPosition = transform.position;

        if (movingRight)
        {
            currentPosition.x += moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPosition.x -= moveSpeed * Time.deltaTime;
        }
        transform.position = currentPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding with left limit");
        if(collision.gameObject.tag == "cloudDirectionChanger")
        {
            movingRight = !movingRight;
        }
    }
}
