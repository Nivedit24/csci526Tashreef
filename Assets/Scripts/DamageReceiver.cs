using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    private int maxHealth = 100;
    private int currHealth;
    private bool isInvincible = false;
    private float invicibilityDuration = 1f;
    private float invincibilityDeltaTime = 0.1f;
    private Vector3 initScale; // Since model scale is not initScale, we use this instead

    public HealthModifier healthBar;

    public GameObject model;

    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        currHealth = maxHealth;
        initScale = model.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // For player flashing effect
    private void ScaleModelTo(Vector3 scale)
    {
        model.transform.localScale = scale;
    }

    // I-Frame calculation on taking damage
    private IEnumerator InvincibilityFrame()
    {
        isInvincible = true;

        // Generating multiple I-frames
        for (float i = 0; i < invicibilityDuration; i += invincibilityDeltaTime)
        {
            if (model.transform.localScale == initScale) ScaleModelTo(Vector3.zero); // Hiding player
            else ScaleModelTo(initScale); // Restoring player

            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        ScaleModelTo(initScale); // To prevent player disappearing with 0 scale
        isInvincible = false;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currHealth -= damage;
        healthBar.SetHealth(currHealth);
        
        /*
        if (currHealth <= 0)
        {
            PlayerController player = model.GetComponentInParent<PlayerController>();
            player.canMove = false;
            FindObjectOfType<GameManager>().GameOver();
            return;
        }
        */
        StartCoroutine(InvincibilityFrame());
    }
}
