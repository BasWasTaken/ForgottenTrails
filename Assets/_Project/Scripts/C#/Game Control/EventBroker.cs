namespace Bas.ForgottenTrails.Events
{
    /// <summary>
    /// Handles communication between classes without them having to etablish a connection first.
    /// </summary>
    public static class EventBroker
    {
        public delegate void GameEndAction(bool victory);
        public static event GameEndAction GameEndEvent;
        public static void GameEndTrigger(bool victory)
        {
            GameEndEvent?.Invoke(victory);
        }
    }
}
