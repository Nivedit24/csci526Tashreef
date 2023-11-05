using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FreezeUnfreezeObject : MonoBehaviour
{
    //public bool isFrozen;
    public float timeFrozen = 5f;
    public Sprite frozenSprite;
    private Sprite initialSprite;

    private SpriteRenderer spriteRenderer;

    private IceMonster_Movement icemonster_mov;
    private EnemyMovement enemyMovement;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
        if (gameObject.tag == "IceMonster")
            icemonster_mov = GetComponent<IceMonster_Movement>();
        else
            enemyMovement = GetComponent<EnemyMovement>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyFrozenAppearanceIceMonster()
    {
        if (frozenSprite != null)
        {
            spriteRenderer.sprite = frozenSprite;
            transform.gameObject.tag = "Untagged";
            gameObject.GetComponent<Collider2D>().isTrigger = false;
        }

        StartCoroutine(UnfreezeAfterDelay(timeFrozen));

    }

    public IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.tag == "Demon" || gameObject.tag == "EarthMonster")
        {
            enemyMovement.isFrozen = false;
            enemyMovement.speed = 10f; // Set speed to its absolute value
            spriteRenderer.sprite = initialSprite;
            enemyMovement.OnEnable();
        }
        else
        {
            gameObject.tag = "IceMonster";
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            icemonster_mov.isFrozen = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        spriteRenderer.sprite = initialSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSnowBall")
        {
            switch (gameObject.tag)
            {
                case "EarthMonster":

                    break;

                case "IceMonster":

                    break;
            }
        }
    }
}
