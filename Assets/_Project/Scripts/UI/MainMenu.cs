using Games.GrumpyBear.LevelManagement;
using UnityEngine;

namespace DualityGame.UI
{
    public class MainMenu: MenuBase
    {
        [Header("New Game")]
        [SerializeField] private SceneGroup _firstLocation;

        [Header("Quit")]
        [SerializeField] private MenuBase _quitDialog;
        
        public void NewGame() => _firstLocation.Load();

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
