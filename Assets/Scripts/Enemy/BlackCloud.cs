using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCloud : MonoBehaviour
{
    public Transform lightningSpawnPoint;
    public GameObject lighningPrefab;
    public float lightningSpeed;
    private float lightningTimer = 0f;
    public float spawnInterval;
    private bool movingRight = false;
    public float moveSpeed = 5f;
    public Vector3 shotSize;
    public bool isStatic = false;

    // Update is called once per frame

    void Update()
    {
        lightningTimer += Time.deltaTime;

        if (lightningTimer >= spawnInterval)
        {
            var bullet = Instantiate(lighningPrefab, lightningSpawnPoint.position, lightningSpawnPoint.rotation);
            if (shotSize.x != 0 || shotSize.y != 0 || shotSize.z != 0)
            {
                bullet.transform.localScale = shotSize;
            }
            bullet.GetComponent<Rigidbody2D>().velocity = (-1) * lightningSpawnPoint.up * lightningSpeed;
            lightningTimer = 0f;
        }

        if (!isStatic)
        {
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


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "cloudDirectionChanger")
        {
            movingRight = !movingRight;
        }
    }
}
