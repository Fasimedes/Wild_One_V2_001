namespace SOSCSRPG.Core
{
    /// <summary>
    /// Event arguments for game messages.
    /// </summary>
    public class GameMessageEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the game message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessageEventArgs"/> class with the specified message.
        /// </summary>
        /// <param name="message">The game message.</param>
        public GameMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}