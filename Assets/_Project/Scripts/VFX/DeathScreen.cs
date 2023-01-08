using System.Collections;
using DualityGame.Core;
using Games.GrumpyBear.Core.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(UIDocument))]
    public class DeathScreen : MonoBehaviour
    {
        public static readonly Games.GrumpyBear.Core.Observables.Observable<string> CauseOfDeath = new();

        [SerializeField] private VoidEvent _onDeathScreen;
        [SerializeField] private float _deathScreenDelay = 3f;

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

        public void Trigger() => StartCoroutine(Wrap());

        private IEnumerator Wrap()
        {
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
            
            PlayState.Current.Set(PlayState.State.Moving);
        }


        private void IncTransitionCount(ITransitionEvent evt) => _runningTransitions++;
        private void DecTransitionCount(ITransitionEvent evt) => _runningTransitions--;

        private void Hide() => _root.AddToClassList("FadeOut");
        private void Show() => _root.RemoveFromClassList("FadeOut");

        private void OnEnable() => CauseOfDeath.Subscribe(OnMessageChange);

        private void OnDisable() => CauseOfDeath.Unsubscribe(OnMessageChange);

        private void OnMessageChange(string msg) => _label.text = msg;
    }
}
