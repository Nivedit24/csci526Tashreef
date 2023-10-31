using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMonster_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    private int currIndex = 0;
    public bool isFrozen = false;
    public float moveRangeX = 3;
    public float moveRangeY = 0;
    public float timeFrozen = 5f;
    //Public Variables
    public Vector2[] setPoints;
    public float movingSpeed = 1.0f;
    private SpriteRenderer frozenSpriteRenderer;
    public Sprite frozenSprite;
    private SpriteRenderer spriteRenderer;
    public GameObject monster;
    private Sprite originalSprite;

    void Start()
    {
        originalSprite = spriteRenderer.sprite;
        setPoints[0] = new Vector2(monster.transform.position.x, monster.transform.position.y);
        generatePoints();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSnowBall")
        {
            isFrozen = true;
            monster.layer = LayerMask.NameToLayer("Ground");
            ApplyFrozenAppearance();
            StartCoroutine(UnfreezeAfterDelay(timeFrozen)); // Unfreeze
            Destroy(collision.gameObject);
        }
    }

    void ApplyFrozenAppearance()
    {
        if (frozenSprite != null)
        {
            spriteRenderer.sprite = frozenSprite;
            monster.tag = "Untagged";
            monster.GetComponent<Collider2D>().isTrigger = false;
        }
    }

    IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        monster.tag = "IceMonster";
        monster.GetComponent<Collider2D>().isTrigger = true;
        isFrozen = false;
        monster.layer = LayerMask.NameToLayer("Default");
        spriteRenderer.sprite = originalSprite;
    }
}
