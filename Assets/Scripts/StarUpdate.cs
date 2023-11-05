using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public float curScale = 0.25f;
    private bool check = true;
    private bool active = true;
    public int requiredActivations;
    public GameObject[] earthMonsterArray = null;

    void Start()
    {
        requiredActivations = earthMonsterArray == null ? 0 : earthMonsterArray.Length;
    }

    // Update is called once per frame
    void Update()
    {
        curScale = check ? curScale + 0.0006f : curScale - 0.0006f;
        if (curScale >= 0.5 || curScale <= 0.25)
            check = !check;

        transform.localScale = new Vector3(curScale, curScale, 1.0f);
        active = IsChallengeCompleted();
        transform.gameObject.GetComponent<Renderer>().material.color = active ? Color.white : Color.black;
        transform.gameObject.GetComponent<Collider2D>().enabled = active;
    }

    private bool IsChallengeCompleted()
    {
        if (earthMonsterArray != null)
        {
            foreach (GameObject earthMonster in earthMonsterArray)
            {
                if (earthMonster.activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
