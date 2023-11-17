using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
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
    public EnemyFreezeTimer enemyfreeze;
    public Canvas freezebarCanvas;
    public bool isUnfreezingCoroutineRunning = false;

    public Coroutine unfreezeAfterDelay;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
        if (gameObject.tag == "IceMonster")
        {
            icemonster_mov = GetComponent<IceMonster_Movement>();
            enemyfreeze = GetComponent<EnemyFreezeTimer>();
            freezebarCanvas = enemyfreeze.freezeCanvas;
            enemyfreeze.enabled = false;
        }

        else {
            
            enemyMovement = GetComponent<EnemyMovement>();
            enemyfreeze = GetComponent<EnemyFreezeTimer>();
            freezebarCanvas = enemyfreeze.freezeCanvas;
            enemyfreeze.enabled = false;
            Debug.Log("demon freeze canvas:" + enemyfreeze.enabled);
        }

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void ApplyFrozenAppearanceIceMonster()
    {
        //if (frozenSprite != null)
        //{
        
            enemyfreeze.enabled = true;
            freezebarCanvas.enabled = true;
            enemyfreeze.HealthBar.SetMaxHealth((int)timeFrozen);
            enemyfreeze.currHealth = (int)timeFrozen;
            enemyfreeze.InvokeRepeating("reduceFrozenTime", 1.0f, 1.0f);
            //gameObject.GetComponentInChildren<Canvas>().enabled = true;
            spriteRenderer.sprite = frozenSprite;
            
            transform.gameObject.tag = "Untagged";
            gameObject.GetComponent<Collider2D>().isTrigger = false;

            
        //}

        unfreezeAfterDelay =   StartCoroutine(UnfreezeAfterDelay(timeFrozen));

    }

    public IEnumerator UnfreezeAfterDelay(float delay)
    {
        

        isUnfreezingCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
        
        if (gameObject.tag == "Demon" || gameObject.tag == "EarthMonster")
        {
            enemyMovement.isFrozen = false;
            enemyMovement.speed = 10f; // Set speed to its absolute value
            spriteRenderer.sprite = initialSprite;
            enemyMovement.OnEnable();
            //gameObject.GetComponentInChildren<Canvas>().enabled = false;
            freezebarCanvas.enabled = false;
            enemyfreeze.CancelInvoke();
            enemyfreeze.currHealth = (int)5f;
            enemyMovement.unFreezeEnemy = null;

        }
        else
        {
            gameObject.tag = "IceMonster";
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            icemonster_mov.isFrozen = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            //gameObject.GetComponentInChildren<Canvas>().enabled = false;
            freezebarCanvas.enabled = false;
            enemyfreeze.CancelInvoke();
            enemyfreeze.currHealth = (int)timeFrozen;
            unfreezeAfterDelay = null;
        }
        spriteRenderer.sprite = initialSprite;
        isUnfreezingCoroutineRunning = false;
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
