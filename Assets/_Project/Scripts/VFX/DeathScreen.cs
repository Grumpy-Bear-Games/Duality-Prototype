using System.Collections;
using DualityGame.UI;
using Games.GrumpyBear.Core.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(UIDocument))]
    public class DeathScreen : MonoBehaviour
    {
        [SerializeField] private VoidEvent _onDeathScreen;
        [SerializeField] private float _deathScreenDelay = 3f;
        [SerializeField] private UnityEvent _onFinished;

        private Label _label;
        private VisualElement _root;
        
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _label = _root.Q<Label>();
            Hide();
        }

        public void Trigger(string causeOfDeath) => StartCoroutine(Trigger_CO(causeOfDeath));

        private IEnumerator Trigger_CO(string causeOfDeath)
        {
            _label.text = causeOfDeath;

            using var transitionMonitor = new TransitionMonitor(_root);

            Show();
            yield return transitionMonitor.WaitUntilDone();
            
            _onDeathScreen.Invoke();
            yield return new WaitForSeconds(_deathScreenDelay);

            Hide();
            yield return transitionMonitor.WaitUntilDone();
            
            _onFinished.Invoke();
        }

        private void Hide() => _root.AddToClassList("FadeOut");
        private void Show() => _root.RemoveFromClassList("FadeOut");
    }
}
