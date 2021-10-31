using UnityEngine.SceneManagement;
using UnityEngine;

// TODO: Refactor to be used as general menu button functionality thingy.
public class LevelSelector : MonoBehaviour
{
    public Animator animator;

    private string levelname;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Select("Open Screen");
        }
    }

    public void Select(string levelname)
    {
        Time.timeScale = 1.0f;
        animator.SetTrigger("Fade Out");
        this.levelname = levelname;
    }

    public void Reset()
    {
        Time.timeScale = 1.0f;
        animator.SetTrigger("Fade Out");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelname);
    }

    public void Quit()
    {
        Time.timeScale = 1.0f;
        Application.Quit();
    }
}
