using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPrefab : MonoBehaviour
{
    public float life = 20;
    public float speed = 20.0f;
    public AcidDropToBlock adtbScript;
    // Start is called before the first frame update
    void Start()
    {
        if (adtbScript == null)
        {
            adtbScript = FindObjectOfType<AcidDropToBlock>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (adtbScript == null)
        {
            adtbScript = FindObjectOfType<AcidDropToBlock>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSnowBall")
        {
            if (transform.gameObject.tag != "AcidBlock")
            {
                transform.gameObject.GetComponent<AcidDropToBlock>().ApplyFrozenAppearance();
                transform.gameObject.tag = "AcidBlock";
                transform.GetComponent<BoxCollider2D>().size = new Vector2(3f, 3f);
                transform.GetComponent<Rigidbody2D>().gravityScale = 20f;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }

        else if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "PlayerFireball")
        {
            Destroy(gameObject);
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hitting fireball to acid block should melt it.
        if (collision.gameObject.tag == "PlayerFireball" && transform.gameObject.tag == "AcidBlock")
        {
            Destroy(gameObject);
        }

    }
}
