using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    public GameObject projectilePrefab;
    public Transform[] LaunchPoints;

    public bool isFrozen = false;
    public Sprite frozenSprite;
    private Sprite initialSprite;

    private SpriteRenderer spriteRenderer;
    private FreezeUnfreezeObject freeze;
    // Update is called once per frame

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
        freeze = GetComponent<FreezeUnfreezeObject>();

    }
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void OnDisable()
    {
        // Cancel the InvokeRepeating when the GameObject is deactivated
        if (gameObject.tag == "Demon" || gameObject.tag == "EarthMonster")
        {
            CancelInvoke("LaunchProjectiles");
        }
    }

    public void OnEnable()
    {
        if (gameObject.tag == "Demon" || gameObject.tag == "EarthMonster")
        {
            InvokeRepeating("LaunchProjectiles", 0f, 3.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Edge") || collision.gameObject.CompareTag("Wall"))
        {
            speed = -speed;
        }

        //Ignore collision with other enemies and spikes
        if (collision.gameObject.CompareTag("Airball") || collision.gameObject.CompareTag("DeathFloor") || collision.gameObject.CompareTag("BreakWall"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSnowBall" && !isFrozen)
        {
            isFrozen = true;
            spriteRenderer.sprite = freeze.frozenSprite;
            speed = 0;
            StartCoroutine(freeze.UnfreezeAfterDelay(5f));
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

}
