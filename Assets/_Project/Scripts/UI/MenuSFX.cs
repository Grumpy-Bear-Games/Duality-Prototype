using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(AudioSource))]
    public class MenuSfx : MonoBehaviour
    {
        [Header("UI Sound Effects")]
        [SerializeField] private AudioClip _navigationMove;
        [SerializeField] private AudioClip _navigationSubmit;
        [SerializeField] private AudioClip _navigationCancel;

        [SerializeField] private AudioClip _buttonClick;


        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            var root = GetComponent<UIDocument>().rootVisualElement;

            root.RegisterCallback<NavigationMoveEvent>(_ => _audioSource.PlayOneShot(_navigationMove), TrickleDown.TrickleDown);
            root.RegisterCallback<NavigationSubmitEvent>(_ => _audioSource.PlayOneShot(_navigationSubmit), TrickleDown.TrickleDown);
            root.RegisterCallback<NavigationCancelEvent>(_ => _audioSource.PlayOneShot(_navigationCancel), TrickleDown.TrickleDown);

            root.RegisterCallback<PointerDownEvent>(e =>
            {
                if (e.target is not VisualElement visualElement) return;
                if (visualElement.ClassListContains("unity-base-field") || visualElement.ClassListContains("unity-base-field__label") || visualElement.ClassListContains("unity-button"))
                    _audioSource.PlayOneShot(_buttonClick);

            }, TrickleDown.TrickleDown);
        }
    }
}
