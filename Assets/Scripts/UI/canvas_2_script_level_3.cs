using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas_2_script_level_3 : MonoBehaviour
{
    public GameObject textObject1;
    public GameObject energyBall;
    public GameObject iceMonster;
    public GameObject player;
    public GameObject background_1;
    public GameObject pushingCube;
    public bool isDropHit = false;
    public GameObject doorThreeDoorPuzzle;
    public GameObject iceThroughWallInstruction;
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
        if (energyBall.activeSelf == false && player.GetComponent<PlayerMovement>().currPower == Power.Water && !isMonsterFrozen)
        {
            text1.text = "SPACEBAR \n Shoot";
            background_1.SetActive(true);
        }
        else if (energyBall.activeSelf == false && player.GetComponent<PlayerMovement>().currPower != Power.Water)
        {
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

        if (!isDropHit)
        {
            pushingCube.SetActive(false);
        }
        else
        {
            pushingCube.SetActive(true);
        }

        if (doorThreeDoorPuzzle.activeSelf)
        {
            iceThroughWallInstruction.SetActive(true);
        }
        else
        {
            iceThroughWallInstruction.SetActive(false);
        }
    }
}
