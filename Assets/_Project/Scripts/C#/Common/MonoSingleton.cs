using UnityEngine;

namespace Bas.Common
{
    /// <summary>
    /// <para>Base Class for creating Singleton-variants of <see cref="MonoBehaviour"/>s, meaning that only one instance is active at any time and this instance can easily be reached using a static field.</para>
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        ///___VARIABLES___///

        #region Properties

        public static T Instance { get; private set; }

        #endregion Properties

        ///___METHODS___///

        #region Protected Methods

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.Log(string.Format("Previously existing instance of {0} class already detected. Destroying presently waking instance called {1}.", GetType(), name));
                Destroy(gameObject);
            }
        }

        #endregion Protected Methods
    }
}