using System.Collections;
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
        private int _runningTransitions;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            _root.RegisterCallback<TransitionRunEvent>(IncTransitionCount);
            _root.RegisterCallback<TransitionEndEvent>(DecTransitionCount);
            _root.RegisterCallback<TransitionCancelEvent>(DecTransitionCount);

            _label = _root.Q<Label>();
            Hide();
        }

        public void Trigger(string causeOfDeath) => StartCoroutine(Trigger_CO(causeOfDeath));

        private IEnumerator Trigger_CO(string causeOfDeath)
        {
            _label.text = causeOfDeath;

            Show();
            do {
                yield return null;
            } while (_runningTransitions > 0);
            
            _onDeathScreen.Invoke();
            yield return new WaitForSeconds(_deathScreenDelay);

            Hide();
            do {
                yield return null;
            } while (_runningTransitions > 0);
            
            _onFinished.Invoke();
        }


        private void IncTransitionCount(ITransitionEvent evt) => _runningTransitions++;
        private void DecTransitionCount(ITransitionEvent evt) => _runningTransitions--;

        private void Hide() => _root.AddToClassList("FadeOut");
        private void Show() => _root.RemoveFromClassList("FadeOut");
    }
}
