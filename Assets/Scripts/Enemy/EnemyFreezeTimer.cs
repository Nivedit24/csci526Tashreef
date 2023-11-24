using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezeTimer : MonoBehaviour
{
    public int currHealth;
    private float frozenTime;
    public HealthModifier freezeBar;
    void Start()
    {
        frozenTime = gameObject.GetComponent<FreezeUnfreezeObject>().timeFrozen;
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
