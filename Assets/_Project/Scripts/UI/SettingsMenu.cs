using Games.GrumpyBear.Core.Settings;
using Games.GrumpyBear.Core.Settings.UIElements;
using Games.GrumpyBear.FMOD.Utilities;
using Games.GrumpyBear.FMOD.Utilities.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private VideoSettings _videoSettings;
        [SerializeField] private BusVolumePreference _masterVolumePreference;
        [SerializeField] private VCAVolumePreference _sfxVolumePreference;
        [SerializeField] private VCAVolumePreference _musicVolumePreference;

        private MainMenu _mainMenu;
        
        private VisualElement _frame;

        private void Awake()
        {
            _mainMenu = GetComponent<MainMenu>();

            var root = GetComponent<UIDocument>().rootVisualElement;

            _frame = root.Q<VisualElement>("SettingsMenu");

            var audioSettings = root.Q<VisualElement>("AudioSettings");
            audioSettings.Q<VolumeSlider>("MasterVolume").VolumePreference = _masterVolumePreference;
            audioSettings.Q<VolumeSlider>("SFXVolume").VolumePreference = _sfxVolumePreference;
            audioSettings.Q<VolumeSlider>("MusicVolume").VolumePreference = _musicVolumePreference;
            
            var videoSettings = root.Q<VisualElement>("VideoSettings");
            videoSettings.Q<VideoSettingsControl>("Fullscreen").VideoSettings = _videoSettings;
            videoSettings.Q<VideoSettingsControl>("Resolution").VideoSettings = _videoSettings;
            videoSettings.Q<VideoSettingsControl>("Quality").VideoSettings = _videoSettings;

            root.Q<Button>("BackButton").clicked += BackButtonClicked;
            
            Hide();
        }

        private void BackButtonClicked()
        {
            Hide();
            _mainMenu.Show();
        }

        public void Show() => _frame.RemoveFromClassList("Hide");

        public void Hide() => _frame.AddToClassList("Hide");
    }
}
