using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas2_script : MonoBehaviour
{
    public GameObject textObject1;
    public GameObject textObject2;
    public GameObject textObject3;

    public GameObject energyBall;
    public GameObject enegyBallNearCloud;
    public GameObject checkpointFirst;
    public GameObject checkpointLast;
    public GameObject Hoverball;
    public GameObject barrier;
    public GameObject background_1;
    public GameObject background_2;
    public GameObject background_3;

    private TextMeshProUGUI text1;
    private TextMeshProUGUI text2;
    private TextMeshProUGUI text3;


    // Start is called before the first frame update
    void Start()
    {
        //text1 = textObject1.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        text1 = textObject1.GetComponent<TextMeshProUGUI>();
        text2 = textObject2.GetComponent<TextMeshProUGUI>();
        text3 = textObject3.GetComponent<TextMeshProUGUI>();

        if (energyBall.activeSelf)
        {
            text1.text = "Collect Energy";
            background_1.SetActive(true);
        }
        else if (energyBall.activeSelf == false && Hoverball.activeSelf == false && checkpointFirst.activeSelf == true)
        {
            text1.text = "SPACEBAR \n Activate Wind";
            background_1.SetActive(true);
        }
        else if (energyBall.activeSelf == false && Hoverball.activeSelf == true && checkpointFirst.activeSelf == true)
        {
            text1.text = "Try Jumping";
            background_1.SetActive(true);
        }
        else
        {
            text1.text = "";
            background_1.SetActive(false);
        }


        if (enegyBallNearCloud.activeSelf == false)
        {
            text3.text = "SPACEBAR \n Activate Wind";
        }


        if (Hoverball.activeSelf == true && checkpointFirst.activeSelf != true)
        {
            text2.text = "SPACEBAR \n Deactivate Wind \n Save Energy";
            background_2.SetActive(true);
        }
        else
        {
            text2.text = "";
            background_2.SetActive(false);
        }

    }
}
