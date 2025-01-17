using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Extensions.QQ.Messages.Payloads.Elements;

namespace Arcbot.Console.Formatters;

public class MessagePayloadFormatter : IConsoleFormatter
{
    public Type Accepted { get; } = typeof(IMessagePayload);

    public void Format(TextWriter writer, object value, Func<object, string> formatter)
    {
        switch (value)
        {
            case RichContent rich:
            {
                AnsiColorHelper.WriteColored(writer, "\"", ConsoleColor.Green);
                foreach (var element in rich.Elements)
                {
                    writer.Write(formatter(element));
                }

                AnsiColorHelper.WriteColored(writer, "\"", ConsoleColor.Green);
                break;
            }
            default:
                writer.Write($"[{value.GetType().Name}]");
                break;
        }
    }
}