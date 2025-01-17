namespace Arcbot.Console;

public static class AnsiColorHelper
{
    public static string DefaultForegroundColorEscapeCode { get; } = "\e[39m\e[22m";
    public static string DefaultBackgroundColorEscapeCode { get; } = "\e[49m";

    public static string GetForegroundColorEscapeCode(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => "\e[30m",
            ConsoleColor.DarkRed => "\e[31m",
            ConsoleColor.DarkGreen => "\e[32m",
            ConsoleColor.DarkYellow => "\e[33m",
            ConsoleColor.DarkBlue => "\e[34m",
            ConsoleColor.DarkMagenta => "\e[35m",
            ConsoleColor.DarkCyan => "\e[36m",
            ConsoleColor.Gray => "\e[37m",
            ConsoleColor.Red => "\e[1m\e[31m",
            ConsoleColor.Green => "\e[1m\e[32m",
            ConsoleColor.Yellow => "\e[1m\e[33m",
            ConsoleColor.Blue => "\e[1m\e[34m",
            ConsoleColor.Magenta => "\e[1m\e[35m",
            ConsoleColor.Cyan => "\e[1m\e[36m",
            ConsoleColor.White => "\e[1m\e[37m",
            _ => DefaultForegroundColorEscapeCode
        };
    }

    public static string GetBackgroundColorEscapeCode(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => "\e[40m",
            ConsoleColor.DarkRed => "\e[41m",
            ConsoleColor.DarkGreen => "\e[42m",
            ConsoleColor.DarkYellow => "\e[43m",
            ConsoleColor.DarkBlue => "\e[44m",
            ConsoleColor.DarkMagenta => "\e[45m",
            ConsoleColor.DarkCyan => "\e[46m",
            ConsoleColor.Gray => "\e[47m",
            _ => DefaultBackgroundColorEscapeCode
        };
    }
    
    public static void WriteColored(TextWriter writer, string text, ConsoleColor? foreground = null,
        ConsoleColor? background = null)
    {
        if (background.HasValue)
        {
            writer.Write(AnsiColorHelper.GetBackgroundColorEscapeCode(background.Value));
        }

        if (foreground.HasValue)
        {
            writer.Write(AnsiColorHelper.GetForegroundColorEscapeCode(foreground.Value));
        }

        writer.Write(text);
        if (foreground.HasValue)
        {
            writer.Write(AnsiColorHelper.DefaultForegroundColorEscapeCode);
        }

        if (background.HasValue)
        {
            writer.Write(AnsiColorHelper.DefaultBackgroundColorEscapeCode);
        }
    }
}