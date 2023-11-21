using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FreezeUnfreezeObject : MonoBehaviour
{
    public float timeFrozen = 5f;
    public Sprite frozenSprite;
    private Sprite initialSprite;
    private SpriteRenderer spriteRenderer;
    private IceMonster_Movement icemonster_mov;
    private EnemyMovement enemyMovement;
    private PlayerMovement playerMovement;
    public EnemyFreezeTimer enemyfreeze;
    public Coroutine unfreezeAfterDelay;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (gameObject.tag == "IceMonster")
        {
            icemonster_mov = GetComponent<IceMonster_Movement>();
            enemyfreeze = GetComponent<EnemyFreezeTimer>();
            initialSprite = spriteRenderer.sprite;
            enemyfreeze.enabled = false;
        }
        else if (gameObject.tag == "Player")
        {
            playerMovement = GetComponent<PlayerMovement>();
            enemyfreeze = GetComponent<EnemyFreezeTimer>();
            enemyfreeze.enabled = false;
        }
        else
        {
            enemyMovement = GetComponent<EnemyMovement>();
            enemyfreeze = GetComponent<EnemyFreezeTimer>();
            initialSprite = spriteRenderer.sprite;
            enemyfreeze.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void ApplyFrozenAppearanceIceMonster()
    {
        enemyfreeze.enabled = true;
        enemyfreeze.freezeBar.gameObject.SetActive(true);
        enemyfreeze.freezeBar.SetMaxHealth((int)timeFrozen);
        enemyfreeze.currHealth = (int)timeFrozen;
        enemyfreeze.InvokeRepeating("reduceFrozenTime", 1.0f, 1.0f);
        spriteRenderer.sprite = frozenSprite;

        transform.gameObject.tag = "Untagged";
        gameObject.GetComponent<Collider2D>().isTrigger = false;

        unfreezeAfterDelay = StartCoroutine(UnfreezeAfterDelay(timeFrozen));
    }

    public IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnFreeze();
    }

    public void UnFreeze()
    {
        if (gameObject.tag == "Demon" || gameObject.tag == "EarthMonster" || gameObject.tag == "BossMonster")
        {
            enemyMovement.isFrozen = false;
            enemyMovement.speed = 10f; // Set speed to its absolute value
            spriteRenderer.sprite = initialSprite;
            enemyMovement.OnEnable();
            enemyfreeze.freezeBar.gameObject.SetActive(false);
            enemyfreeze.CancelInvoke();
            enemyfreeze.currHealth = (int)5f;
            enemyMovement.unFreezeEnemy = null;
            spriteRenderer.sprite = initialSprite;
        }
        else if (gameObject.tag == "Player")
        {
            playerMovement.isFrozen = false;
            enemyfreeze.freezeBar.gameObject.SetActive(false);
            playerMovement.transform.Find("ice_cube").gameObject.SetActive(false);
            enemyfreeze.CancelInvoke();
            enemyfreeze.currHealth = (int)5f;
            playerMovement.speed = playerMovement.speedDuplicate;
            playerMovement.jumpSpeed = playerMovement.jumpSpeedDuplicate;
            playerMovement.unFreezeEnemy = null;
        }
        else
        {
            gameObject.tag = "IceMonster";
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            icemonster_mov.isFrozen = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            enemyfreeze.freezeBar.gameObject.SetActive(false);
            enemyfreeze.CancelInvoke();
            enemyfreeze.currHealth = (int)timeFrozen;
            unfreezeAfterDelay = null;
            spriteRenderer.sprite = initialSprite;
        }
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
