using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBarController : MonoBehaviour
{
    public float maxFreezeDuration = 5.0f; // Maximum freeze duration
    private float remainingTime; // Time remaining until unfreezing
    private Slider progressBar; // Reference to the UI Slider component

    void Start()
    {
        progressBar = GetComponent<Slider>();
        progressBar.value = 0; // Initialize the progress bar to 0 (empty)
    }

    public void StartTimer(float freezeDuration)
    {
        remainingTime = freezeDuration;
        StartCoroutine(UpdateProgressBar());
    }

    IEnumerator UpdateProgressBar()
    {
        while (remainingTime > 0)
        {
            float fillAmount = 1 - (remainingTime / maxFreezeDuration);
            progressBar.value = fillAmount;
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        progressBar.value = 0; // Ensure the bar is completely filled when the timer is done
    }
}
