using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas2_script_level2 : MonoBehaviour
{
    public GameObject textObject1;
    public GameObject energyBall;
    public GameObject fireLogo;
    public GameObject enemyDemon;

    private TextMeshProUGUI text1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text1 = textObject1.GetComponent<TextMeshProUGUI>();
        if(energyBall.activeSelf == false && fireLogo.activeSelf == true && enemyDemon.activeSelf == true)
        {
            text1.text = "Space - Shoot Fireball";
        }
        else if(energyBall.activeSelf == false && fireLogo.activeSelf == false)
        {
            text1.text = "X - Select Fire";
        }
        else
        {
            text1.text = "";
        }

    }
}
