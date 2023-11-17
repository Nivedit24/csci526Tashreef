using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezeTimer : MonoBehaviour
{
    public int currHealth;
    public float frozenTime = 10f;
    //public Canvas freezeCanvas;
    public HealthModifier freezeBar;
    public int CHILD_INDEX = 1;
    void Start()
    {
        frozenTime = gameObject.GetComponent<FreezeUnfreezeObject>().timeFrozen;
        Debug.Log("Time frozen: " + frozenTime);
        freezeBar.SetMaxHealth((int)(frozenTime));
        currHealth = (int)(frozenTime);
    }

    // Update is called once per frame
    public void reduceFrozenTime()
    {
        currHealth -= 1;
        freezeBar.SetHealth(currHealth);
    }
}
