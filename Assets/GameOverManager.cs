using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    public string levelsSceneName = "Levels"; // The name of your game over scene

    public void LoadLevelsScene ()
    {
        Debug.Log("Levels");
        SceneManager.LoadScene(levelsSceneName);
    }

    public void LoadNextLevel ()
    {
        Debug.Log("Levels");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +  1);
    }
}


