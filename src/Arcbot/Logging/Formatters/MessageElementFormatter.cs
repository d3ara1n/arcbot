using System;
using Ac682.Extensions.Logging.Console;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using Spectre.Console;

namespace Arcbot.Logging.Formatters
{
    public class MessageElementFormatter: IObjectLoggingFormatter
    {
        public bool IsTypeAvailable(Type type) => type.IsAssignableTo(typeof(MessageElement));

        public string Format(object obj, Type type, string format = null)
        {
            var ele = (MessageElement)obj;
            return ele switch
            {
                Plain it => $"[green]\"{Markup.Escape(it.Text)}\"[/]",
                _ => Markup.Escape(ele.ToString())
            };
        }
    }
}