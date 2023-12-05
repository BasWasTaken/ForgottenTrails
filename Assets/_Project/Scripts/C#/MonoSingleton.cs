using UnityEngine;
namespace Bas.Utility 
{
    /// <summary>
    /// <para>Base Class for creating Singleton-variants of <see cref="MonoBehaviour"/>s, meaning that only one instance is active at any time and this instance can easily be reached using a static field.</para>
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        ///___VARIABLES___///
        #region BACKEND_VARIABLES
        public static T Instance { get; private set; }
        #endregion
        ///___METHODS___///
        #region LIFESPAN
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
        #endregion
    }
}
