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
    [SerializeField] public List<GameObject> boulderPlatforms;
    void Start()
    {
        if (boulderPlatforms == null)
            requiredActivations = 0;
        else
        {
            requiredActivations = boulderPlatforms.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        curScale = check ? curScale + 0.0006f : curScale - 0.0006f;
        if (curScale >= 0.5 || curScale <= 0.25)
            check = !check;

        transform.localScale = new Vector3(curScale, curScale, 1.0f);

        int currActivations = GetActiveBoulderPlatforms();
        Debug.Log("currActivations: " + currActivations);
        active = requiredActivations <= currActivations;
        transform.gameObject.GetComponent<Renderer>().material.color = active ? Color.white : Color.black;
        transform.gameObject.GetComponent<Collider2D>().enabled = active;
    }

    private int GetActiveBoulderPlatforms()
    {
        if (boulderPlatforms == null)
            return 0;

        int currentActivations = 0;

        for (int i = 0; i < boulderPlatforms.Count; i++)
        {
            GameObject platform = boulderPlatforms[i].transform.gameObject;
            if (platform.GetComponent<BoulderPlatform>().activated == true)
            {
                currentActivations += 1;
            }
        }
        return currentActivations;
    }
}
