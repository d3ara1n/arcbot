using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Messages.ConcreteModels;
using Hyperai.Messages.ConcreteModels.FileSources;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;
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

        [Receive(MessageEventType.Group)]
        [Description("看看别人 Github")]
        [Extract("!github.detailed {query}")]
        public async Task Query(Group group, string query)
        {
            string owner;
            string name;
            if (query.Contains('/'))
            {
                owner = query.Substring(0,query.IndexOf('/'));
                name = query.Substring(owner.Length + 1);

                var repository = await _client.Repository.Get(owner, name);

                StringBuilder builder = new();
                if (repository.Archived) builder.Append("[Archived]");
                builder.AppendLine($"{repository.FullName}");
                builder.AppendLine($"  --{repository.Description}");
                builder.AppendLine($"Language: {repository.Language}");
                builder.AppendLine(
                    $"Star/Watch/Fork: {repository.StargazersCount}/{repository.WatchersCount}/{repository.ForksCount}");
                builder.AppendLine($"CreatedAt: {repository.CreatedAt}");
                builder.AppendLine($"PushedAt: {repository.PushedAt}");
                builder.Append($"Url: {repository.HtmlUrl}");

                await group.SendPlainAsync(builder.ToString().Trim());
            }
            else
            {
                owner = query;

                var user = await _client.User.Get(owner);

                MessageChainBuilder chainBuilder = new();
                StringBuilder builder = new();
                chainBuilder.AddImage(null, new UrlSource(new Uri(user.AvatarUrl)));
                
                builder.AppendLine($"[{user.Login}]{user.Name}");
                builder.AppendLine($"  --{user.Bio ?? "(NULL)"}");
                builder.AppendLine($"Email: {user.Email ?? "(NULL)"}");
                builder.AppendLine($"Followers/Following: {user.Followers}/{user.Following}");
                builder.AppendLine($"CreatedAt: {user.CreatedAt}");
                builder.AppendLine($"{user.HtmlUrl}");
                
                chainBuilder.AddPlain(builder.ToString().Trim());

                await group.SendAsync(chainBuilder.Build());
            }
        }
        
        [Receive(MessageEventType.Group)]
        [Extract("!github {query}")]
        [Description("获取仓库的图片描述")]
        public async Task Github(Group group, string query)
        {
            MessageChain chain = MessageChain.Construct(new Image(null,
                new UrlSource(new Uri($"https://opengraph.githubassets.com/0/{query}"))));
            await group.SendAsync(chain);
        }
    }
}