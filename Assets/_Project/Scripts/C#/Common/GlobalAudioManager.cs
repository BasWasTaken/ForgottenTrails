namespace Bas.Common
{
    /// <summary>
    /// <para>Manages Audio Globally by proving an instance of an <see cref="Bas.Utility.AudioHandler"/> everyone can access.</para>
    /// </summary>
    public class GlobalAudioManager : MonoSingleton<GlobalAudioManager>
    {
        // Public Properties

        #region Properties

        public AudioHandler Global { get; private set; }

        #endregion Properties

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        #endregion Protected Methods
    }
}