using Bas.Utility;

namespace ForgottenTrails
{
    /// <summary>
    /// <para>Manages Audio Globally by proving an instance of an <see cref="Bas.Utility.AudioHandler"/> everyone can access.</para>
    /// </summary>
    public class GlobalAudioManager : MonoSingleton<GlobalAudioManager>
    {
        // Public Properties
        #region Public Properties
        public AudioHandler Global { get; private set; }
        #endregion
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
