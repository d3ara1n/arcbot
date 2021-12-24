using System.Threading.Tasks;
using HyperaiX;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using Quartz;

namespace Arcbot.Jobs;

public class HelloJob: IJob
{
    private readonly IApiClient _client;
    public HelloJob(IApiClient client)
    {
        _client = client;
    }
    public Task Execute(IJobExecutionContext context)
    {
        var isGroup = (bool)context.MergedJobDataMap["IsGroup"];
        var group = (long)context.MergedJobDataMap["Group"];
        var user = (long)context.MergedJobDataMap["User"];

        return isGroup
            ? _client.SendGroupMessageAsync(group, MessageChain.Construct(new Plain("Hello from 15 secs ago")))
            : _client.SendFriendMessageAsync(user, MessageChain.Construct(new Plain("Hello from 15 secs ago")));
    }
}