using System;
namespace SOSCSRPG.Core
{
    /// <summary>
    /// Message broker class to handle message passing using the Singleton design pattern.
    /// </summary>
    public class MessageBroker
    {
        // Static singleton instance of MessageBroker
        private static readonly MessageBroker s_messageBroker = new MessageBroker();

        /// <summary>
        /// Private constructor to implement the Singleton pattern.
        /// </summary>
        private MessageBroker()
        {
        }

        /// <summary>
        /// Event that is raised when a message is sent.
        /// </summary>
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        /// <summary>
        /// Gets the singleton instance of the MessageBroker.
        /// </summary>
        /// <returns>The singleton instance of MessageBroker.</returns>
        public static MessageBroker GetInstance()
        {
            return s_messageBroker;
        }

        /// <summary>
        /// Raises a message event with the specified message.
        /// </summary>
        /// <param name="message">The message to raise.</param>
        public void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}