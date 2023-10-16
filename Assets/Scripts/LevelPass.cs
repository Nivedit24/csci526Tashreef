using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().buildIndex <= 3)
            {
                //Add Checkpoint Analytics Code
                PlayerMovement pm = gameObject.AddComponent<PlayerMovement>();
                pm.callCheckPointTimeAnalyticsLevelChange(SceneManager.GetActiveScene().buildIndex);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

    }
}
