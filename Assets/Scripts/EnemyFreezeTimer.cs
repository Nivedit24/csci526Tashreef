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
    void Start()
    {
        freezeCanvas = gameObject.GetComponentInChildren<Canvas>();
        freezeCanvas.enabled = false;
        frozenTime = gameObject.GetComponent < FreezeUnfreezeObject >().timeFrozen;
        Debug.Log("Time frozen: "+frozenTime);
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
