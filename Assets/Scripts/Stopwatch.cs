using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Stopwatch : MonoBehaviour
{
    float currentTime;
    bool stopwatchActive = false;
    [SerializeField] private TMP_Text currentTimeText;

    [SerializeField] private TMP_Text bestTimeText;
    private float bestTimeValue;
    private Scene scene;

    void Start()
    {
        currentTime = 0;

        // Get best time from playerprefs for the current scene and apply it to the best time text.
        scene = SceneManager.GetActiveScene();
        bestTimeValue = PlayerPrefs.GetFloat(scene.name, 3599.999f); // If no best time set to 30 minutes.
        TimeSpan time = TimeSpan.FromSeconds(bestTimeValue);
        bestTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    void Update()
    {
        if (stopwatchActive)
        {
            currentTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    public void StartStopwatch()
    {
        stopwatchActive = true;
    }

    public void StopStopwatch()
    {
        stopwatchActive = false;

        // If current time is less than the best time, update in playerprefs.
        if (currentTime < bestTimeValue)
        {
            PlayerPrefs.SetFloat(scene.name, currentTime);
        }
    }
}
