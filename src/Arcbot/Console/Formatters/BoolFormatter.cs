using System.Runtime.Serialization;

namespace Arcbot.Console.Formatters;

public class BoolFormatter : IConsoleFormatter
{
    public Type Accepted { get; } = typeof(bool);

    public void Format(TextWriter writer, object value, Func<object, string> formatter)
    {
        AnsiColorHelper.WriteColored(writer, value.ToString() ?? string.Empty,
            ConsoleColor.Magenta);
    }
}