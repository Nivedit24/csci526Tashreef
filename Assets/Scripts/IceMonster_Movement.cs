using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMonster_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    private int currIndex = 0;

    private bool isFrozen = false;
    public float moveRangeX = 3;
    public float moveRangeY = 0;
    //Public Variables
    public Vector2[] setPoints;
    public float movingSpeed = 1.0f;
    private SpriteRenderer frozenSpriteRenderer;
    public Sprite frozenSprite;
    private SpriteRenderer spriteRenderer;

    private Sprite originalSprite;
    void Start()
    {
        originalSprite = spriteRenderer.sprite;
        setPoints[0] = new Vector2(GameObject.Find("ice-monster").transform.position.x, GameObject.Find("ice-monster").transform.position.y);
        generatePoints();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    void Awake()
    {
        spriteRenderer = GetComponent < SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFrozen) { 
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
        for(int i =1; i<2;i++)
        {
            float randomx =  GameObject.Find("ice-monster").transform.position.x + moveRangeX;
            float randomy = GameObject.Find("ice-monster").transform.position.y + moveRangeY;

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
            StartCoroutine(UnfreezeAfterDelay(5f)); // Unfreeze
        }
    }
    void ApplyFrozenAppearance()
    {
        if (frozenSprite != null)
        {
            spriteRenderer.sprite = frozenSprite;
        }
    }

    IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFrozen = false;
        spriteRenderer.sprite = originalSprite;
    }
}
