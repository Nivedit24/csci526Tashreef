using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas2_script_level2 : MonoBehaviour
{
    public GameObject textObject1;
    public GameObject textObject2;
    public GameObject energyBall;
    public GameObject player;
    public GameObject fireLogo;
    public GameObject airLogo;
    public GameObject enemyDemon;

    public GameObject background_1;
    public GameObject background_2;

    private TextMeshProUGUI text1;
    private TextMeshProUGUI text2;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        text1 = textObject1.GetComponent<TextMeshProUGUI>();
        text2 = textObject2.GetComponent<TextMeshProUGUI>();
        if(energyBall.activeSelf == false && player.GetComponent<PlayerMovement>().currPower == Power.Fire && enemyDemon.activeSelf == true)
        {
            text1.text = "SPACEBAR \n Shoot Fireball";
            background_1.SetActive(true);
        }
        else if(energyBall.activeSelf == false && player.GetComponent<PlayerMovement>().currPower != Power.Fire)
        {
            text1.text = "Press X \n Select Fire";
            background_1.SetActive(true);
        }
        else
        {
            text1.text = "";
            background_1.SetActive(false);
        }

        if(enemyDemon.activeSelf == false && player.GetComponent<PlayerMovement>().isHovering == false)
        {
            text2.text = "Press Z \n Select Wind";
            background_2.SetActive(true);
        }
        else
        {
            text2.text = "";
            background_2.SetActive(false);
        }

    }
}
