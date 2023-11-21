using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas_2_script_level_4 : MonoBehaviour
{
    public GameObject textObject1;
    public GameObject textObject2;
    public GameObject energyBall;
    public GameObject earthMonster;
    public GameObject player;
    public GameObject background_1;
    public GameObject background_2;

    private TextMeshProUGUI text1, text2;

    // Update is called once per frame
    void Update()
    {
        text1 = textObject1.GetComponent<TextMeshProUGUI>();
        text2 = textObject2.GetComponent<TextMeshProUGUI>();

        if (energyBall.activeSelf == false && player.GetComponent<PlayerMovement>().currPower != Power.Earth)
        {
            text1.text = "Press V \n Select Earth";
            background_1.SetActive(true);
        }
        else
        {
            text1.text = "";
            background_1.SetActive(false);
        }

        if (earthMonster.activeSelf)
        {
            text2.text = "Kill Monster \n Unlock Star";
            background_2.SetActive(true);
        }
        else
        {
            text2.text = "";
            background_2.SetActive(false);
        }
    }
}
