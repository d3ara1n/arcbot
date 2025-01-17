using HyperaiX.Abstractions.Roles;
using HyperaiX.Extensions.QQ.Roles;

namespace Arcbot.Console.Formatters;

public class UserFormatter : IConsoleFormatter
{
    public Type Accepted { get; } = typeof(IUser);

    public void Format(TextWriter writer, object value, Func<object, string> formatter)
    {
        switch (value)
        {
            case Friend friend:
                AnsiColorHelper.WriteColored(writer, "Friend", ConsoleColor.Yellow);
                AnsiColorHelper.WriteColored(writer, "(", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, friend.Id.ToString());
                AnsiColorHelper.WriteColored(writer, ", ", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, friend.DisplayName);
                AnsiColorHelper.WriteColored(writer, ")", ConsoleColor.Gray);
                break;
            case Member member:
                AnsiColorHelper.WriteColored(writer, "Member", ConsoleColor.Blue);
                AnsiColorHelper.WriteColored(writer, "(", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, member.Id.ToString());
                AnsiColorHelper.WriteColored(writer, ", ", ConsoleColor.Gray);
                AnsiColorHelper.WriteColored(writer, member.DisplayName);
                AnsiColorHelper.WriteColored(writer, ")", ConsoleColor.Gray);
                break;
            default:
                writer.Write(value);
                break;
        }
    }
}