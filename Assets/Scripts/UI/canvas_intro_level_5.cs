using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas_intro_level_5 : MonoBehaviour
{
    public GameObject[] blinkingStarsList = null;
    public GameObject textObject1;
    private TextMeshProUGUI text1;
    public GameObject barrierForBossBattle;


    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        text1 = textObject1.GetComponent<TextMeshProUGUI>();
        if (areRequiredStarsCollected())
        {
            text1.text = "";
            barrierForBossBattle.SetActive(false);
        }
        else
        {
            text1.text = "Collect All 11 \n \n To Unlock";
            barrierForBossBattle.SetActive(true);
        }
    }

    private bool areRequiredStarsCollected()
    {
        if (blinkingStarsList != null)
        {
            foreach (GameObject star in blinkingStarsList)
            {
                if (star.activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
