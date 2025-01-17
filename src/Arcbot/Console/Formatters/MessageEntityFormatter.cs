using HyperaiX.Abstractions.Messages;

namespace Arcbot.Console.Formatters;

public class MessageEntityFormatter : IConsoleFormatter
{
    public Type Accepted { get; } = typeof(MessageEntity);

    public void Format(TextWriter writer, object value, Func<object, string> formatter)
    {
        if (value is MessageEntity entity)
        {
            writer.Write(formatter(entity.Body));
        }
        else
        {
            writer.Write(value);
        }
    }
}