using UnityEngine;
using UnityEngine.SceneManagement;

namespace RehabiliTEA
{
    public class LevelLoader : MonoBehaviour
    {
        private string levelToLoad;

        public void LoadLevel()
        {
            if (!Profile.GetProfile().HasInternet) SceneManager.LoadScene(levelToLoad);
        }

        public void SetNextLevel(string nextLevel)
        {
            if (Profile.GetProfile().HasInternet)
            {
                Profile.GetProfile().LoadDifficulty(nextLevel);
                SceneManager.LoadScene(nextLevel);
            }
            else
            {
                levelToLoad = nextLevel;
            }
        }
    }
}