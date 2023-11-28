using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 100;
    public int currHealth;
    public bool isBlinking = false;
    private float invicibilityDuration = 1f;
    private float invincibilityDeltaTime = 0.1f;

    public HealthModifier healthBar;

    public GameObject model;
    public PlayerMovement playerMovement;

    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // For player flashing effect

    private IEnumerator InvincibilityFrame()
    {
        isBlinking = true;

        for (float i = 0; i < invicibilityDuration; i += invincibilityDeltaTime)
        {
            model.transform.GetComponent<PlayerAnimation>().TogglePlayerVisibility();

            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        model.transform.GetComponent<PlayerAnimation>().SetPlayerInvisible(false); // To prevent player disappearing with 0 scale
        isBlinking = false;
    }

    public void TakeDamage(int damage, bool shielded)
    {
        if (isBlinking || shielded) return;

        currHealth -= damage;
        healthBar.SetHealth(currHealth);
        if (currHealth <= 0)
        {
            playerMovement.KillPlayer();
        }
        StartCoroutine(InvincibilityFrame());
    }

    public void giveHealth()
    {
        healthBar.SetMaxHealth(maxHealth);
        currHealth = maxHealth;
    }
}
