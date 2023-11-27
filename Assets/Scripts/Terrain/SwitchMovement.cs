using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMovement : MonoBehaviour
{
    public GameObject switches;
    public GameObject platform;
    public bool activated;
    public float startPositionY;
    public int direction = 1;
    public float speed = 10.0f;
    public float rangeYUp = 10.0f;
    public float rangeYDown = 10.0f;
    public bool allowResetToParentPlatform = false;
    void Start()
    {
        startPositionY = platform.transform.position.y;
        if (switches)
        {
            switches.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    Transform fetchPlayerIfPresentAsChild()
    {
        Transform platformHolder = platform.transform.Find("PlatformHolder");
        if (platformHolder)
        {
            return platformHolder.Find("Player");
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = fetchPlayerIfPresentAsChild();
        if (activated)
        {
            if (platform.transform.position.y > startPositionY + rangeYUp)
            {
                direction = -1;
            }
            else if (platform.transform.position.y < startPositionY - rangeYDown)
            {
                direction = 1;
            }
            platform.transform.Translate(Vector3.up * speed * Time.deltaTime * direction);
            if (player)
            {
                player.gameObject.GetComponent<PlayerMovement>().parentPlarformDirection = direction;
                player.gameObject.GetComponent<PlayerMovement>().parentPlatformSpeed = speed;
            }
        }
        if (switches)
        {
            switches.GetComponent<Renderer>().material.color = activated ? Color.green : Color.red;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PlayerFireball")
        {
            activated = !activated;
        }
    }
}
