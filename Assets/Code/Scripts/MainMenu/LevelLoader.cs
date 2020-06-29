using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private string levelToLoad;

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SetNextLevel(string nextLevel)
    {
        levelToLoad = nextLevel;
    }
}
