using Bas.Utility;

namespace ForgottenTrails
{
    /// <summary>
    /// <para>Manages Audio Globally by proving an instance of an <see cref="Bas.Utility.AudioManager"/> everyone can access.</para>
    /// </summary>
    public class GlobalAudioManager : MonoSingleton<GlobalAudioManager>
    {
        // Public Properties
        #region Public Properties
        public AudioManager Global { get; private set; }
        #endregion
    }
}
