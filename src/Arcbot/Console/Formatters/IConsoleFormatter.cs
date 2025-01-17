namespace Arcbot.Console.Formatters;

public interface IConsoleFormatter
{
    Type Accepted { get; }
    void Format(TextWriter writer, object value, Func<object, string> formatter);
}