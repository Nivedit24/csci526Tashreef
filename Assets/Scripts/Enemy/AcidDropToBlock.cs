using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidDropToBlock : MonoBehaviour
{
    private bool isFrozen = false;
    public Sprite frozenSprite;
    private SpriteRenderer spriteRenderer;
    public Sprite originalSprite;
    public float timeFrozen = 5.0f;
    private GameObject canvasIntro = null;
    private GameObject player;
    void Start()
    {
        originalSprite = spriteRenderer.sprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFrozen = false;
        player = GameObject.Find("Player");
    }
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSnowBall")
        {
            canvasIntro = GameObject.Find("CanvasIntro");
            if (canvasIntro && canvasIntro.GetComponent<canvas_2_script_level_3>())
            {
                canvasIntro.GetComponent<canvas_2_script_level_3>().isDropHit = true;
            }
            if (!isFrozen)
            {
                isFrozen = true;
                ApplyFrozenAppearance();
                StartCoroutine(UnfreezeAfterDelay(timeFrozen));
                Destroy(collision.gameObject);
            }
        }
    }

    public void ApplyFrozenAppearance()
    {
        if (frozenSprite != null)
        {
            spriteRenderer.sprite = frozenSprite;
            transform.gameObject.GetComponent<Collider2D>().isTrigger = false;
            player.GetComponent<PlayerMovement>().iceCubes.Add(transform.gameObject);
        }
    }

    IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.gameObject.GetComponent<Collider2D>().isTrigger = true;
        isFrozen = false;
        spriteRenderer.sprite = originalSprite;
    }
}

