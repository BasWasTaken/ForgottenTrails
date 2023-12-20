using System;
using System.Collections;
using UnityEngine;

namespace Bas.Common.Extensions
{
    /// <summary>
    /// Extension class that allows for simple access to delayed actions.
    /// </summary>
    public static class CoroutineExtensions
    {
        ///___METHODS___///
        #region DelayedActions
        /// Executes an action after a specified time has elapsed
        public static Coroutine DelayedAction(this MonoBehaviour monoBehaviour, Action action, float secondsDelay)
        {
            return monoBehaviour.StartCoroutine(DelayedActionCoroutine(action, secondsDelay));
        }
        private static IEnumerator DelayedActionCoroutine(Action action, float duration)
        {
            yield return new WaitForSeconds(duration);

            action();
        }

        public static Coroutine DelayedAction(this MonoBehaviour monoBehaviour, Action action, float secondsDelay, params bool[] conditions)
        {
            return monoBehaviour.StartCoroutine(DelayedActionCoroutine(action, secondsDelay));
        }
        private static IEnumerator DelayedActionCoroutine(Action action, float duration, params bool[] conditions)
        {
            yield return new WaitForSeconds(duration);
            foreach (bool condition in conditions)
            {
                yield return new WaitUntil(() => condition);
            }

            action();
        }
        #endregion
    }
}
