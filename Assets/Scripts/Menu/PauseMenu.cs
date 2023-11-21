using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject completionText;
    private bool isGameCompleted = false;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        if (completionText)
        {
            if (completionText.activeSelf)
            {
                isGameCompleted = true;
            }
            completionText.SetActive(false);
        }
        Time.timeScale = 0;
    }
    public void Home()
    {
        SceneManager.LoadScene("LevelSelector");
        Time.timeScale = 1;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        if (completionText && isGameCompleted)
        {
            completionText.SetActive(true);
        }
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
