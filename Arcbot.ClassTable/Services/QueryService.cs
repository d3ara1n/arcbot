using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.Plugins;

namespace Arcbot.ClassTable.Services
{
    public class QueryService
    {
        private readonly IRepository _repository;

        public QueryService(IPluginRepository<PluginEntry> repository)
        {
            _repository = repository.Value;
        }
    }
}