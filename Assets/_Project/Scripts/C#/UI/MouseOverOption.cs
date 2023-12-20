namespace Bas.ForgottenTrails.UI
{

    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>

    public interface IMouseOverOption
    {
        bool IsMouseOver { get; set; }
        void OnMouseEnter();
        void OnMouseExit();
        void UpdateWhenMouseOver();
    }
}
