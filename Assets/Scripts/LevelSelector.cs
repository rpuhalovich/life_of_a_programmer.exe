using UnityEngine.SceneManagement;
using UnityEngine;

// TODO: Refactor to be used as general menu button functionality thingy.
public class LevelSelector : MonoBehaviour
{
    public void Select(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
