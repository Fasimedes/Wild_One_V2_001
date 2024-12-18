﻿using System;
namespace SOSCSRPG.Core
{
    public class MessageBroker
    {
        // Use the Singleton design pattern for this class,
        // to ensure everything in the game sends messages through this one object.
        private static readonly MessageBroker s_messageBroker =
            new MessageBroker();
        private MessageBroker()
        {
        }
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;
        public static MessageBroker GetInstance()
        {
            return s_messageBroker;
        }
        public void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}