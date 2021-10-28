using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private static bool isPaused = false;
    [SerializeField] private GameObject pauseMenuUI;

    [SerializeField] private PlayerController player; // Used to mute player sounds when paused.

    [SerializeField] private LevelSelector levelSelector;

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
        levelSelector.Quit();
        Debug.Log("Quit");
    }

    public void LevelSelect()
    {
        levelSelector.Select("Level Select");
        Debug.Log("Level Select");
    }

    public void SetSensitivity(float value)
    {
        player.SetSensitivity(value);
    }
}
