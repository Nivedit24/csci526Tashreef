using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas_2_script_level_3 : MonoBehaviour
{
    public GameObject textObject1;
    public GameObject textObject2;
    public GameObject energyBall;
    public GameObject iceMonster;
    public GameObject energyBall2;
    public GameObject player;
    public GameObject background_1;
    public GameObject background_2;
    public bool isDropHit = false;
    private bool isMonsterFrozen = false;
    

    private TextMeshProUGUI text1, text2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text1 = textObject1.GetComponent<TextMeshProUGUI>();
        text2 = textObject2.GetComponent<TextMeshProUGUI>();
        if (energyBall.activeSelf == false && player.GetComponent<PlayerMovement>().currPower == Power.Water && !isMonsterFrozen)
        {
            text1.text = "SPACEBAR \n Shoot";
            background_1.SetActive(true);
        }
        else if (energyBall.activeSelf == false && player.GetComponent<PlayerMovement>().currPower != Power.Water)
        {
            Debug.Log("Ice Instructions");
            text1.text = "Press C \n Select Ice";
            background_1.SetActive(true);
        }
        else if (energyBall.activeSelf == false && isMonsterFrozen)
        {
            text1.text = "Jump on \n Frozen \n Monster";
            background_1.SetActive(true);
        }
        else
        {
            text1.text = "";
            background_1.SetActive(false);
        }

        if (iceMonster.GetComponent<IceMonster_Movement>().isFrozen)
        {
            isMonsterFrozen = true;
        }
        else
        {
            isMonsterFrozen = false;
        }

        if(energyBall2.activeSelf == false && !isDropHit)
        {
            text2.text = "Shoot a falling drop";
            background_2.SetActive(true);
        }
        else if(energyBall2.activeSelf == false)
        {
            text2.text = "Good! Now Push \n the frozen cube";
            background_2.SetActive(true);
        }
        else
        {
            text2.text = "";
            background_2.SetActive(false);
        }

    }
}
