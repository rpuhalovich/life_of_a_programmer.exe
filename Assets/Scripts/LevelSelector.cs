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
        animator.SetTrigger("Fade Out");
        this.levelname = levelname;
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelname);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
