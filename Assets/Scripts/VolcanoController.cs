using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] LaunchPoints;
    public float launchInterval = 5f;   // Time interval between each launch
    public float projectileSpeed = 5f;
    public Sprite frozenSprite;
    private Sprite initialSprite;

    private SpriteRenderer spriteRenderer;
    private bool isFrozen = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
        InvokeRepeating("LaunchProjectiles", 0f, launchInterval);
    }

    void LaunchProjectiles()
    {
        if (!isFrozen)
        {
            for (int i = 0; i < LaunchPoints.Length; i++)
                Instantiate(projectilePrefab, LaunchPoints[i].position, LaunchPoints[i].rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSnowBall" && !isFrozen)
        {
            isFrozen = true;

            // Change the sprite to the frozen sprite
            spriteRenderer.sprite = frozenSprite;

            StartCoroutine(StopAndRestartProjectiles());
            Destroy(collision.gameObject);
        }
    }

    IEnumerator StopAndRestartProjectiles()
    {
        // Stop launching projectiles for 5 seconds
        isFrozen = true;
        yield return new WaitForSeconds(5f);

        spriteRenderer.sprite = initialSprite;

        // Restart launching projectiles
        isFrozen = false;
    }
}
