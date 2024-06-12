using System;
using System.Threading;
using UnityEngine;

namespace DualityGame.Core
{
    /// <summary>
    /// CustomYieldInstruction that waits until all subscribers have completed their work.
    /// </summary>
    public class WaitForCompletion : CustomYieldInstruction
    {
        private int _pendingSubscribers;

        /// <summary>
        /// Creates an IDisposable that, when disposed, will signal the completion of work by a subscriber.
        /// </summary>
        /// <returns>An IDisposable that the subscriber should dispose upon completion.</returns>
        public IDisposable CreateCompletionTrigger()
        {
            Interlocked.Increment(ref _pendingSubscribers);
            return new CompletionTrigger(this);
        }

        /// <summary>
        /// Keeps waiting as long as there are pending subscribers.
        /// </summary>
        public override bool keepWaiting => _pendingSubscribers > 0;

        private void TriggerCompleted() => Interlocked.Decrement(ref _pendingSubscribers);

        private class CompletionTrigger : IDisposable
        {
            private readonly WaitForCompletion _waitForCompletion;
            private bool _isDisposed;

            public CompletionTrigger(WaitForCompletion waitForCompletion) => _waitForCompletion = waitForCompletion;

            public void Dispose()
            {
                if (_isDisposed) return;
                _waitForCompletion.TriggerCompleted();
                _isDisposed = true;
            }
        }
    }
}
