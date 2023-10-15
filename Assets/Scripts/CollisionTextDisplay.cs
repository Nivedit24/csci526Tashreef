using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollisionTextDisplay : MonoBehaviour
{
    public TMP_Text displayText;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")  // Change the tag as needed.
        {
            DisplayText("Use Shift to shoot in player's direction");
        }
    }

    private void DisplayText(string message)
    {
        displayText.text = message;
        Invoke("HideTextAfterDelay", 3f);
    }

    void HideTextAfterDelay()
    {
        displayText.text = "";
    }
}
