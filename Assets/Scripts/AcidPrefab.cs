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
    //void Awake()
    //{
    //    Destroy(gameObject, life);
    //}

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
        if (collision.gameObject.tag == "PlayerFireball")
        {
            Debug.Log("Collided with snowball from acid prefab");

            transform.gameObject.GetComponent<AcidDropToBlock>().ApplyFrozenAppearance();
            transform.gameObject.tag = "Untagged";
        }

        else if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("Collided with ground form acid prefab");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        //Destroy(gameObject);
    }

    
}
