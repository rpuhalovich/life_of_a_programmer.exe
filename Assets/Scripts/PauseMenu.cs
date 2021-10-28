using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public Slider sensitivitySlider;
    public Slider musicSlider;
    public PlayerController player; // Used to mute player sounds when paused.

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        player.SetPausedStatus(false);
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Pause()
    {
        player.SetPausedStatus(true);
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void Quit()
    {
        Debug.Log("Quitting game...");
        // Application.Quit();
    }

    public void LevelSelect()
    {
        Debug.Log("Level select...");
    }

    public void SetSensitivity(float value)
    {
        player.SetSensitivity(value);
    }
}
