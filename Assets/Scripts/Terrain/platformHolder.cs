using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformHolder : MonoBehaviour
{
    private Transform acidBlockParent;
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
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.transform.SetParent(transform);
                break;
            case "AcidBlock":
                acidBlockParent = collision.gameObject.transform.parent;
                collision.gameObject.transform.SetParent(transform);
                break;
            case "Switch":
                collision.gameObject.transform.SetParent(transform);
                break;
            case "Shield":
                collision.transform.parent.gameObject.transform.SetParent(transform);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
              other.gameObject.transform.SetParent(null);
                other.gameObject.GetComponent<PlayerMovement>().parentPlarformDirection = 0;
                other.gameObject.GetComponent<PlayerMovement>().parentPlatformSpeed = 0;
                break;
            case "AcidBlock":
                if (other.gameObject.activeSelf)
                {
                    other.gameObject.transform.SetParent(acidBlockParent);
                }
                break;
            case "Shield":
                if (other.gameObject.activeSelf)
                {
                    Transform player = other.transform.parent;
                    player.gameObject.transform.SetParent(null);
                    player.gameObject.GetComponent<PlayerMovement>().parentPlarformDirection = 0;
                    player.gameObject.GetComponent<PlayerMovement>().parentPlatformSpeed = 0;
                }
                break;
        }
    }
}
