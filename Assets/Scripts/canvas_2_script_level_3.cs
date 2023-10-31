using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canvas_2_script_level_3 : MonoBehaviour
{
    public GameObject textObject1;
    public GameObject textObject2;
    public GameObject energyBall;
    public GameObject iceLogo;
    public GameObject iceMonster;
    public GameObject energyBall2;
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
        if (energyBall.activeSelf == false && iceLogo.activeSelf == true && !isMonsterFrozen)
        {
            text1.text = "Shoot the Monster";
        }
        else if (energyBall.activeSelf == false && iceLogo.activeSelf == false)
        {
            Debug.Log("Ice Instructions");
            text1.text = "C - Select Ice";
        }
        else if (energyBall.activeSelf == false && isMonsterFrozen)
        {
            text1.text = "Jump on Frozen \n Monster";
        }
        else
        {
            text1.text = "";
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
        }
        else if(energyBall2.activeSelf == false)
        {
            text2.text = "Good! Now Push \n the frozen cube";
        }
        else
        {
            text2.text = "";
        }

    }
}
