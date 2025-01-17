using HyperaiX.Abstractions.Events;

namespace Arcbot.Console.Formatters;

public class EventFormatter : IConsoleFormatter
{
    public Type Accepted { get; } = typeof(GenericEventArgs);

    public void Format(TextWriter writer, object value, Func<object, string> formatter)
    {
        switch (value)
        {
            case MessageEventArgs message:
                AnsiColorHelper.WriteColored(writer, "Message", ConsoleColor.DarkYellow);
                AnsiColorHelper.WriteColored(writer, "(", ConsoleColor.Gray);
                writer.Write(formatter(message.Chat));
                AnsiColorHelper.WriteColored(writer, ", ", ConsoleColor.Gray);
                writer.Write(formatter(message.Sender));
                AnsiColorHelper.WriteColored(writer, "): ", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, formatter(message.Message));
                break;
            default:
                AnsiColorHelper.WriteColored(writer, value.GetType().Name, ConsoleColor.DarkYellow);
                break;
        }
    }
}