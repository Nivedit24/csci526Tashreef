using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public float curScale = 1f;
    private bool check = true;
    private bool active = true;
    public int requiredActivations;
    public GameObject[] enemyArray = null;

    void Start()
    {
        requiredActivations = enemyArray == null ? 0 : enemyArray.Length;
        ToggleEnemyStars(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //changing this added value
        curScale = check ? curScale + 0.02f : curScale - 0.02f;
        if (curScale >= 1.5 || curScale <= 0.5)
            check = !check;

        transform.localScale = new Vector3(curScale, curScale, 1.0f);
        active = IsChallengeCompleted();
        transform.gameObject.GetComponent<Renderer>().material.color = active ? Color.white : Color.gray;
        transform.gameObject.GetComponent<Collider2D>().enabled = active;
    }

    private bool IsChallengeCompleted()
    {
        if (enemyArray != null)
        {
            foreach (GameObject enemy in enemyArray)
            {
                if (enemy.activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ToggleEnemyStars(bool active)
    {
        if (enemyArray != null)
        {
            foreach (GameObject enemy in enemyArray)
            {
                Transform star = enemy.transform.Find("star");
                if (star != null)
                {
                    star.gameObject.SetActive(active);
                }
            }
        }
    }
}
