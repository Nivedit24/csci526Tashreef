using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenIceMonster : MonoBehaviour
{// Start is called before the first frame update
    private int currIndex = 0;
    private bool isFrozen = false;
    public float moveRangeX = 3;
    public float moveRangeY = 0;
    public float timeFrozen = 5f;
    public Vector2[] setPoints;
    public float movingSpeed = 1.0f;
    private SpriteRenderer frozenSpriteRenderer;
    public Sprite frozenSprite;
    private SpriteRenderer spriteRenderer;
    public GameObject monster;
    private Sprite originalSprite;

    public GameObject iceMonster; // Parent GameObject
    public GameObject frozenMonster; // Child GameObject

    void Start()
    {
        originalSprite = spriteRenderer.sprite;
        spriteRenderer = GetComponent < SpriteRenderer>();
        monster = GetComponent<GameObject>();
        // Initialize the positions and activate the ice monster
        InitializeIceMonster();
    }

    void InitializeIceMonster()
    {
        setPoints[0] = new Vector2(monster.transform.position.x, monster.transform.position.y);
        generatePoints();
        iceMonster.SetActive(true);
        frozenMonster.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!isFrozen)
        {
            transform.position = Vector2.MoveTowards(transform.position, setPoints[currIndex], movingSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, setPoints[currIndex]) < 0.02f)
            {
                currIndex++;
                if (currIndex >= setPoints.Length)
                {
                    currIndex = 0;
                }
            }
        }
    }

    void generatePoints()
    {
        // Generate new random points
        for (int i = 1; i < 2; i++)
        {
            float randomx = monster.transform.position.x + moveRangeX;
            float randomy = monster.transform.position.y + moveRangeY;

            setPoints[i] = new Vector2(randomx, randomy);
            Debug.Log(setPoints[i]);
        }
        isFrozen = false;
        spriteRenderer.sprite = originalSprite;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerFireball")
        {
            isFrozen = true;
            ApplyFrozenAppearance();

            // Activate the frozen monster and deactivate the ice monster
            iceMonster.SetActive(false);
            frozenMonster.SetActive(true);

            StartCoroutine(UnfreezeAfterDelay(timeFrozen));
        }
    }


    void ApplyFrozenAppearance()
    {
        if (frozenSprite != null)
        {
            spriteRenderer.sprite = frozenSprite;
            monster.tag = "Untagged";
        }
    }

    IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFrozen = false;

        // Deactivate the frozen monster and activate the ice monster
        iceMonster.SetActive(true);
        frozenMonster.SetActive(false);

        //spriteRenderer.sprite = originalSprite;
        monster.tag = "IceMonster";
        InitializeIceMonster();
    }
}
