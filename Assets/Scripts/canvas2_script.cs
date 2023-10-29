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
    public GameObject checkpointFirst;
    public GameObject checkpointLast;
    public GameObject Hoverball;
    public GameObject barrier;
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
            text1.text = "Energy is your fuel";
        }
        else if (energyBall.activeSelf == false && Hoverball.activeSelf == false && checkpointFirst.activeSelf == true)
        {
            text1.text = "Press Space \n Activate/Deactivate Wind";
        }
        else if (energyBall.activeSelf == false && Hoverball.activeSelf == true && checkpointFirst.activeSelf == true)
        {
            text1.text = "Try Jumping";
        }
        else
        {
            text1.text = "";
        }


        if (energyBall.activeSelf && checkpointFirst.activeSelf != true)
        {
            text2.text = "Energy Exhausted. Go Back to collect.";
        }
        else if (Hoverball.activeSelf == false && checkpointFirst.activeSelf != true)
        {
            text2.text = "Activate Wind";
        }
        else
        {
            text2.text = "";
        }

        if (checkpointLast.activeSelf == false && barrier.activeSelf)
        {
            text3.text = "You Missed Some Stars!";
        }
        else
        {
            text3.text = "";
        }
    }
}
