using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Arcbot.Console.Formatters;

public class FancyConsoleFormatter() : ConsoleFormatter(nameof(FancyConsoleFormatter))
{
    // TODO: 把 Ac682.Extensions.Logging.Console 抄过来优化一下
    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        throw new NotImplementedException();
    }
}