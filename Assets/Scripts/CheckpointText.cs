using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointText : MonoBehaviour
{
    public TMP_Text displayText;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")  // Change the tag as needed.
        {
            DisplayText("Collect stars to checkpoint your progress");
        }
    }

    private void DisplayText(string message)
    {
        displayText.text = message;
        Invoke("HideTextAfterDelay", 5f);
    }

    void HideTextAfterDelay()
    {
        displayText.text = "";
    }
}