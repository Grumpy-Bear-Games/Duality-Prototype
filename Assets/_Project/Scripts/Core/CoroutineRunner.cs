using System.Collections;
using UnityEngine;

namespace DualityGame.Core
{
    public class CoroutineRunner : MonoBehaviour
    {
        public static void Run(IEnumerator coroutine)
        {
            var go = new GameObject("[Coroutine runner]", typeof(CoroutineRunner));
            go.GetComponent<CoroutineRunner>().RunOnce(coroutine);
        }

        private void RunOnce(IEnumerator coroutine) => StartCoroutine(RunOnce_CO(coroutine));

        private IEnumerator RunOnce_CO(IEnumerator coroutine)
        {
            DontDestroyOnLoad(gameObject);
            yield return coroutine;
            Destroy(gameObject);
        }
    }
}
