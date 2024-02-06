namespace VVGames.Common
{
    /// <summary>
    /// <para>Manages Audio Globally by proving an instance of an <see cref="VVGames.Utility.AudioHandler"/> everyone can access.</para>
    /// </summary>
    public class GlobalAudioManager : MonoSingleton<GlobalAudioManager>
    {
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