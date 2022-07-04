using System.Collections.Generic;
using System.Threading.Tasks;
using Arcbot.Services;
using HyperaiX;
using HyperaiX.Abstractions.Relations;
using Quartz;

namespace Arcbot.Jobs;

public class SelfStoreUpdateJob : IJob
{
    private readonly IApiClient _client;
    private readonly SelfStore _store;

    public SelfStoreUpdateJob(SelfStore store, IApiClient client)
    {
        _store = store;
        _client = client;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var groups = new List<Group>();
        var friends = new List<Friend>();

        //TODO: HyperaiX 没有获取这些的 Action

        await Task.CompletedTask;
    }
}