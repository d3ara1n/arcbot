using System.ComponentModel;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using Microsoft.Extensions.Logging;
using Octokit;

namespace Arcbot.Essential.Units
{
    public class GithubUnit: UnitBase
    {
        private readonly ILogger _logger;
        private readonly GitHubClient _client;

        public GithubUnit(ILogger<GithubUnit> logger)
        {
            _logger = logger;
            _client = new GitHubClient(new ProductHeaderValue("Arcbot"));
        }

        // [Receive(MessageEventType.Group)]
        [Description("看看别人 Github")]
        [Extract("!github {query}")]
        public async Task Query(Group group, string query)
        {
            string owner;
            string name;

            MessageChainBuilder builder = new();
            if (query.Contains('/'))
            {
                owner = query.Substring(0,query.IndexOf('/'));
                name = query.Substring(owner.Length + 1);

                var repository = await _client.Repository.Get(owner, name);
            }
            else
            {
                owner = query;

                var user = await _client.User.Get(owner);
            }
            
            
            //_client.Repository.Get();
        }
    }
}