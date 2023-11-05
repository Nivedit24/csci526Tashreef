using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonScript : MonoBehaviour
{
  
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the actual name of your main menu scene
    }
     public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}




