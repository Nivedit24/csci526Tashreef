using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private float rangeX = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if (player.transform.position.x > rangeX) return;

        if (player.transform.position.x > 0)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }
    }
}
