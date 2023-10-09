using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1;
    public DamageReceiver playerReceiver;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerReceiver.TakeDamage(damage);
        }
    }
}
