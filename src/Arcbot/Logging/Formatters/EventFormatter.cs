using System;
using Ac682.Extensions.Logging.Console;
using HyperaiX.Abstractions.Events;

namespace Arcbot.Logging.Formatters
{
    public class EventFormatter : IObjectLoggingFormatter
    {
        public string Format(object obj, Type type, string format = null) => obj switch
        {
            GroupMessageEventArgs it => $"G | Group = {it.Group.Name} Sender = {it.Sender.DisplayName}\n{it.Message.Flatten()}",
            FriendMessageEventArgs it => $"F | Sender = {it.Sender.Nickname}\n{it.Message.Flatten()}",
            _ => obj.ToString()
        };

        public bool IsTypeAvailable(Type type) => type.IsAssignableTo(typeof(GenericEventArgs));
    }
}