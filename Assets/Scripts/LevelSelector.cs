using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public void Select(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }
}
