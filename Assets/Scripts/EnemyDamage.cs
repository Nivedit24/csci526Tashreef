using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthModifier HealthBar;
    public int maxHealth = 100;
    public int currHealth;
    void Start()
    {
        HealthBar.SetMaxHealth(maxHealth);
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        HealthBar.SetHealth(currHealth);
    }
}
