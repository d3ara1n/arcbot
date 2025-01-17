using System.Drawing;
using System.Globalization;
using System.Text;
using Arcbot.Console.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Arcbot.Console;

public class FancyConsoleFormatter : ConsoleFormatter
{
    private readonly FancyConsoleFormatterOptions _options;
    private readonly List<IConsoleFormatter> _formatters = new();

    public FancyConsoleFormatter(IOptions<FancyConsoleFormatterOptions> options) : base(nameof(FancyConsoleFormatter))
    {
        _options = options.Value;
        foreach (var type in options.Value.Formatters)
        {
            if (type.IsAssignableTo(typeof(IConsoleFormatter)))
            {
                if (Activator.CreateInstance(type) is IConsoleFormatter formatter)
                {
                    _formatters.Add(formatter);
                }
            }
        }
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        var time = _options.UseUtcTimestamp ? DateTime.UtcNow : DateTime.Now;
        AnsiColorHelper.WriteColored(textWriter, time.ToString(_options.TimestampFormat), ConsoleColor.Gray);

        (string, ConsoleColor?, ConsoleColor?) level = logEntry.LogLevel switch
        {
            LogLevel.None => ("NONE", ConsoleColor.Gray, null),
            LogLevel.Trace => ("TRAC", ConsoleColor.Cyan, null),
            LogLevel.Debug => ("DEBG", ConsoleColor.DarkMagenta, null),
            LogLevel.Information => ("INFO", ConsoleColor.Green, null),
            LogLevel.Warning => ("WARN", ConsoleColor.Yellow, null),
            LogLevel.Error => ("ERRO", ConsoleColor.Red, null),
            LogLevel.Critical => ("FATA", ConsoleColor.White, ConsoleColor.Red),
            _ => throw new NotImplementedException()
        };
        AnsiColorHelper.WriteColored(textWriter, $" {level.Item1} ", level.Item2, level.Item3);

        var sep = logEntry.Category.LastIndexOf('.');
        if (sep != -1)
        {
            var leading = logEntry.Category[..(sep + 1)];
            var trailing = logEntry.Category[(sep + 1)..];
            AnsiColorHelper.WriteColored(textWriter, leading, ConsoleColor.Gray);
            AnsiColorHelper.WriteColored(textWriter, trailing, ConsoleColor.DarkCyan);
            AnsiColorHelper.WriteColored(textWriter, $"({logEntry.EventId}) ", ConsoleColor.DarkCyan);
        }
        else
        {
            AnsiColorHelper.WriteColored(textWriter, $"{logEntry.Category}({logEntry.EventId}) ",
                ConsoleColor.DarkCyan);
        }

        if (_options.IncludeScopes && scopeProvider != null)
        {
            scopeProvider.ForEachScope((scope, state) =>
                {
                    if (scope != null) AnsiColorHelper.WriteColored(state, $"({scope}) ", ConsoleColor.Gray);
                },
                textWriter);
        }

        if (logEntry.State is IReadOnlyList<KeyValuePair<string, object>> states)
        {
            const string formatKey = "{OriginalFormat}";
            string? format = null;
            var values = new object[states.Count - 1];
            var index = 0;
            foreach (var (key, value) in states)
            {
                if (key == formatKey) format = value.ToString();
                else
                {
                    values[index++] = Format(_formatters, value);
                }
            }

            if (format != null)
            {
                var actual = BuildFormat(format);
                textWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, actual, values));
            }
            else
            {
                textWriter.WriteLine(logEntry.Formatter(logEntry.State, logEntry.Exception));
            }
        }
        else
        {
            textWriter.WriteLine(logEntry.Formatter(logEntry.State, logEntry.Exception));
        }
    }

    private static object Format(IReadOnlyList<IConsoleFormatter> formatters, object value)
    {
        if (formatters.FirstOrDefault(x => x.Accepted.IsAssignableFrom(value.GetType())) is { } formatter)
        {
            var tmp = new StringWriter();
            formatter.Format(tmp, value, v => Format(formatters, v).ToString() ?? string.Empty);
            return tmp.ToString();
        }

        return value;
    }

    #region Copied Code

    // 沟槽的微软，关键的实现类都是 internal sealed
    //https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Abstractions/src/LogValuesFormatter.cs
    private static CompositeFormat BuildFormat(string original)
    {
        var vsb = new StringBuilder();
        int count = 0;
        int scanIndex = 0;
        int endIndex = original.Length;

        while (scanIndex < endIndex)
        {
            int openBraceIndex = FindBraceIndex(original, '{', scanIndex, endIndex);
            if (scanIndex == 0 && openBraceIndex == endIndex)
            {
                // No holes found.
                return CompositeFormat.Parse(original);
            }

            int closeBraceIndex = FindBraceIndex(original, '}', openBraceIndex, endIndex);

            if (closeBraceIndex == endIndex)
            {
                vsb.Append(original.AsSpan(scanIndex, endIndex - scanIndex));
                scanIndex = endIndex;
            }
            else
            {
                // Format item syntax : { index[,alignment][ :formatString] }.
                int formatDelimiterIndex =
                    original.AsSpan(openBraceIndex, closeBraceIndex - openBraceIndex).IndexOfAny(',', ':');
                formatDelimiterIndex =
                    formatDelimiterIndex < 0 ? closeBraceIndex : formatDelimiterIndex + openBraceIndex;

                vsb.Append(original.AsSpan(scanIndex, openBraceIndex - scanIndex + 1));
                // vsb.Append(_valueNames.Count.ToString());
                vsb.Append(count++);
                // _valueNames.Add(original.Substring(openBraceIndex + 1, formatDelimiterIndex - openBraceIndex - 1));
                vsb.Append(original.AsSpan(formatDelimiterIndex, closeBraceIndex - formatDelimiterIndex + 1));

                scanIndex = closeBraceIndex + 1;
            }
        }

        return CompositeFormat.Parse(vsb.ToString());
    }

    private static int FindBraceIndex(string format, char brace, int startIndex, int endIndex)
    {
        // Example: {{prefix{{{Argument}}}suffix}}.
        int braceIndex = endIndex;
        int scanIndex = startIndex;
        int braceOccurrenceCount = 0;

        while (scanIndex < endIndex)
        {
            if (braceOccurrenceCount > 0 && format[scanIndex] != brace)
            {
                if (braceOccurrenceCount % 2 == 0)
                {
                    // Even number of '{' or '}' found. Proceed search with next occurrence of '{' or '}'.
                    braceOccurrenceCount = 0;
                    braceIndex = endIndex;
                }
                else
                {
                    // An unescaped '{' or '}' found.
                    break;
                }
            }
            else if (format[scanIndex] == brace)
            {
                if (brace == '}')
                {
                    if (braceOccurrenceCount == 0)
                    {
                        // For '}' pick the first occurrence.
                        braceIndex = scanIndex;
                    }
                }
                else
                {
                    // For '{' pick the last occurrence.
                    braceIndex = scanIndex;
                }

                braceOccurrenceCount++;
            }

            scanIndex++;
        }

        return braceIndex;
    }

    #endregion
}