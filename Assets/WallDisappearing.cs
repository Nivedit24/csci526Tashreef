using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDisappearing : MonoBehaviour
{
    public string tagToDisappear = "Player"; // Set the tag you want to trigger the wall disappearance.

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagToDisappear))
        {
            // Check if the colliding object has the specified tag.
            // If it does, destroy the wall.
            Destroy(gameObject);
        }
    }
}
