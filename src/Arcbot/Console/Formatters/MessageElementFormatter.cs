using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Extensions.QQ.Messages.Payloads.Elements;

namespace Arcbot.Console.Formatters;

public class MessageElementFormatter : IConsoleFormatter
{
    public Type Accepted { get; } = typeof(IMessageElement);

    public void Format(TextWriter writer, object value, Func<object, string> formatter)
    {
        switch (value)
        {
            case Text text:
                AnsiColorHelper.WriteColored(writer, text.Plain, ConsoleColor.Green);
                break;
            case At at:
                AnsiColorHelper.WriteColored(writer, $"[{at.Display}]", ConsoleColor.Magenta);
                break;
            default:
                AnsiColorHelper.WriteColored(writer, $"[{value.GetType().Name}]", ConsoleColor.Magenta);
                break;
        }
    }
}