using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowBallSelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        if(collisionTag != "AcidDrop" && collisionTag != "IceMonster" && collisionTag!="Demon" && collisionTag != "Untagged")
        {
            Destroy(transform.gameObject);
        }
    }
}
