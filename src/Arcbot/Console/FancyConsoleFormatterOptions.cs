using Arcbot.Console.Formatters;
using Microsoft.Extensions.Logging.Console;

namespace Arcbot.Console;

public class FancyConsoleFormatterOptions: ConsoleFormatterOptions
{
    private List<Type> _formatters = [];
    public IList<Type> Formatters => _formatters;
}