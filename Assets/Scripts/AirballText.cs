using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AirballText : MonoBehaviour
{
    public TMP_Text displayText;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")  // Change the tag as needed.
        {
            DisplayText("Collect airballs to hover through clouds and move more swiftly");
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