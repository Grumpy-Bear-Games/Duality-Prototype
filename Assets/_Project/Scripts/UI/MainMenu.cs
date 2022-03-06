using UnityEngine;
using UnityEngine.SceneManagement;

namespace DualityGame.UI
{
    public class MainMenu: MenuBase
    {
        [SerializeField] private MenuBase _quitDialog;
        
        public void NewGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        public override void Close() => _quitDialog.Open();

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
