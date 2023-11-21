using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    public GameObject door;
    private GameObject player;
    private bool initialDoorState;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if(door != null)
        {
            initialDoorState = door.activeSelf;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void flipDoorState()
    {
        if (door.activeSelf)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AcidBlock") || collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("AcidBlock"))
            {
                player.GetComponent<PlayerMovement>().iceCubesOnDoorSwitches.Add(collision.gameObject);
            }
            door.SetActive(!initialDoorState);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AcidBlock") || collision.gameObject.CompareTag("Player"))
        {
            if (player.GetComponent<PlayerMovement>().iceCubesOnDoorSwitches.Contains(collision.gameObject))
            {
                player.GetComponent<PlayerMovement>().iceCubesOnDoorSwitches.Remove(collision.gameObject);
            }
            door.SetActive(initialDoorState);
        }
    }
}
