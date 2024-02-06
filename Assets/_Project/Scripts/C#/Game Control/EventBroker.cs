namespace VVGames.ForgottenTrails.Events
{
    /// <summary>
    /// Handles communication between classes without them having to etablish a connection first.
    /// </summary>
    public static class EventBroker
    {
        #region Delegates

        public delegate void GameEndAction(bool victory);

        #endregion Delegates

        #region Events

        public static event GameEndAction GameEndEvent;

        #endregion Events

        #region Public Methods

        public static void GameEndTrigger(bool victory)
        {
            GameEndEvent?.Invoke(victory);
        }

        #endregion Public Methods
    }
}