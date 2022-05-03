using Games.GrumpyBear.LevelManagement;
using UnityEngine;

namespace DualityGame.UI
{
    public class MainMenu: MenuBase
    {
        [Header("New Game")]
        [SerializeField] private LocationManager _locationManager;
        [SerializeField] private Location _firstLocation;

        [Header("Quit")]
        [SerializeField] private MenuBase _quitDialog;
        
        public void NewGame() => _locationManager.Load(_firstLocation);

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
