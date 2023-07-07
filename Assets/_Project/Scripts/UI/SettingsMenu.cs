using System;
using DualityGame.Utilities;
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

        public event Action OnHide;
        
        private VisualElement _frame;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            _frame = root.Q<VisualElement>("SettingsMenu");
            _frame.RegisterCallback<NavigationCancelEvent>(_ => BackButtonClicked());
            _frame.PreventLoosingFocus();

            var audioSettings = root.Q<VisualElement>("AudioSettings");
            audioSettings.Q<VolumeSlider>("MasterVolume").VolumePreference = _masterVolumePreference;
            audioSettings.Q<VolumeSlider>("SFXVolume").VolumePreference = _sfxVolumePreference;
            audioSettings.Q<VolumeSlider>("MusicVolume").VolumePreference = _musicVolumePreference;
            
            var videoSettings = root.Q<VisualElement>("VideoSettings");
            videoSettings.Q<FullscreenToggle>("Fullscreen").VideoSettings = _videoSettings;
            videoSettings.Q<ResolutionDropdown>("Resolution").VideoSettings = _videoSettings;
            videoSettings.Q<QualityDropdown>("Quality").VideoSettings = _videoSettings;

            root.Q<Button>("BackButton").clicked += BackButtonClicked;
        }

        private void BackButtonClicked()
        {
            Hide();
            OnHide?.Invoke();
        }

        public void Show()
        {
            _frame.AddToClassList("Shown");
            _frame.Q<VisualElement>("MasterVolume").Q<Slider>().Focus();
        }

        public void Hide() => _frame.RemoveFromClassList("Shown");
    }
}
