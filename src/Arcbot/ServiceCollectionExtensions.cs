using System;
using System.Linq;
using System.Runtime.Loader;
using Arcbot.Models.Events;
using HyperaiX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onebot.Protocol.Models;

namespace Arcbot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiClient(this IServiceCollection services)
    {
        ModelFactory.RegisterEventModel<FriendPokeEvent>("notice.friend_poke");
        ModelFactory.RegisterEventModel<GroupNameUpdateEvent>("notice.group_name_update");
        ModelFactory.RegisterEventModel<NewFriendEvent>("request.new_friend");
        ModelFactory.RegisterEventModel<JoinGroupEvent>("request.join_group");
        ModelFactory.RegisterEventModel<GroupInvitedEvent>("request.group_invited");
        return services
            .AddSingleton<IApiClient, ApiClient>();
    }

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (var type in AssemblyLoadContext.All.SelectMany(x => x.Assemblies.SelectMany(y => y.GetExportedTypes()))
                     .Where(z => z != typeof(ModuleBase) && z.IsAssignableTo(typeof(ModuleBase))))
        {
            var module = Activator.CreateInstance(type) as ModuleBase;
            module!.ConfigureServices(services, configuration);
        }

        return services;
    }
}