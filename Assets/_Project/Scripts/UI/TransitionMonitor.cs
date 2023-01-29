using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    public sealed class TransitionMonitor : IDisposable
    {

        private readonly VisualElement _visualElement;
        private int _runningTransitions;

        public TransitionMonitor(VisualElement visualElement)
        {
            _visualElement = visualElement;
            _visualElement.RegisterCallback<TransitionRunEvent>(IncTransitionCount);
            _visualElement.RegisterCallback<TransitionEndEvent>(DecTransitionCount);
            _visualElement.RegisterCallback<TransitionCancelEvent>(DecTransitionCount);
        }
        
        public void Dispose()
        {
            _visualElement.UnregisterCallback<TransitionRunEvent>(IncTransitionCount);
            _visualElement.UnregisterCallback<TransitionEndEvent>(DecTransitionCount);
            _visualElement.UnregisterCallback<TransitionCancelEvent>(DecTransitionCount);
        }

        public IEnumerator WaitUntilDone()
        {
            do {
                yield return null;
            } while (_runningTransitions > 0);
        }

        private void IncTransitionCount(ITransitionEvent evt) => _runningTransitions++;
        private void DecTransitionCount(ITransitionEvent evt) => _runningTransitions--;
    }
}
