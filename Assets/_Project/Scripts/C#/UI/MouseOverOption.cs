namespace Bas.ForgottenTrails.UI
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>

    public interface IMouseOverOption
    {
        #region Properties

        bool IsMouseOver { get; set; }

        #endregion Properties

        #region Public Methods

        void OnMouseEnter();

        void OnMouseExit();

        void UpdateWhenMouseOver();

        #endregion Public Methods
    }
}