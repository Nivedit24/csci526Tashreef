using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezeTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthModifier HealthBar;
    //public int maxHealth = 100;
    public int currHealth;
    public float frozenTime = 10f;
    public Canvas freezeCanvas;
    public int CHILD_INDEX = 1;
    void Start()
    {
        //if (gameObject.tag == "IceMonster")
        //   freezeCanvas = gameObject.GetComponentInChildren<Canvas>();
        //else
        //    freezeCanvas = transform.Find("CanvasEnemy (1)").gameObject.GetComponent<Canvas>();
        freezeCanvas.enabled = false; ;
        frozenTime = gameObject.GetComponent<FreezeUnfreezeObject>().timeFrozen;
        Debug.Log("Time frozen: " + frozenTime);
        HealthBar.SetMaxHealth((int)(frozenTime));
        currHealth = (int)(frozenTime);
    }

    // Update is called once per frame
    public void reduceFrozenTime()
    {
        currHealth -= 1;
        HealthBar.SetHealth(currHealth);
    }
}
