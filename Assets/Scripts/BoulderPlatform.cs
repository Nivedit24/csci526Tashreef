using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public bool activated = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Boulder") || other.gameObject.CompareTag("Player"))
        {
            activated = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Boulder") || other.gameObject.CompareTag("Player"))
        {
            activated = false;
        }
    }
}
