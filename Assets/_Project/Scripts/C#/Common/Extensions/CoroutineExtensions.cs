// ------------------------------------------------------------------------------
// Created on: Pre-2024.
// Author: Jaep
// Purpose: Simplifying the use of coroutines.
// ------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;

namespace VVGames.Common.Extensions
{
    /// <summary>
    /// Extension class that allows for simple access to delayed actions.
    /// </summary>
    public static class CoroutineExtensions
    {
        #region Public Methods

        // Executes an action after a specified time has elapsed
        public static Coroutine DelayedAction(this MonoBehaviour monoBehaviour, Action action, float secondsDelay)
        {
            return monoBehaviour.StartCoroutine(DelayedActionCoroutine(action, secondsDelay));
        }

        public static Coroutine DelayedAction(this MonoBehaviour monoBehaviour, Action action, float secondsDelay, params bool[] conditions)
        {
            return monoBehaviour.StartCoroutine(DelayedActionCoroutine(action, secondsDelay));
        }

        #endregion Public Methods

        #region Private Methods

        private static IEnumerator DelayedActionCoroutine(Action action, float duration)
        {
            yield return new WaitForSeconds(duration);

            action();
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

        #endregion Private Methods
    }
}