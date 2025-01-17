using HyperaiX.Abstractions.Roles;
using HyperaiX.Extensions.QQ.Roles;

namespace Arcbot.Console.Formatters;

public class ChatFormatter : IConsoleFormatter
{
    public Type Accepted { get; } = typeof(IChat);

    public void Format(TextWriter writer, object value, Func<object, string> formatter)
    {
        switch (value)
        {
            case Conversation conversation:
                AnsiColorHelper.WriteColored(writer, "Conversation", ConsoleColor.Yellow);
                AnsiColorHelper.WriteColored(writer, "(", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, conversation.Conversant.DisplayName);
                AnsiColorHelper.WriteColored(writer, ")", ConsoleColor.Gray);
                break;
            case Group group:
                AnsiColorHelper.WriteColored(writer, "Group", ConsoleColor.Blue);
                AnsiColorHelper.WriteColored(writer, "(", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, group.Id.ToString());
                AnsiColorHelper.WriteColored(writer, ", ", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, group.Name);
                AnsiColorHelper.WriteColored(writer, ")", ConsoleColor.Gray);
                break;
            default:
                writer.Write(value);
                break;
        }
    }
}