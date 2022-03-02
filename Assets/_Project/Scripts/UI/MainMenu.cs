using UnityEngine.SceneManagement;

namespace DualityGame.UI
{
    public class MainMenu : MenuBase
    {
        // Temporary, very naive approach
        public void NewGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }                
    }
}
