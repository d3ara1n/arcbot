using HyperaiX.Abstractions.Events;
using HyperaiX.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Arcbot;

public static class HyperaiConfigurationBuilderExtensions
{
    public static HyperaiXConfigurationBuilder UseEventBlocker(this HyperaiXConfigurationBuilder builder)
    {
        return builder.Use((evt, pvd, nxt) =>
        {
            if (evt is not UnknownEventArgs) nxt(evt, pvd);
        });
    }

    public static HyperaiXConfigurationBuilder UseLogging(this HyperaiXConfigurationBuilder builder)
    {
        return builder.Use((evt, pvd, nxt) =>
        {
            var logger = pvd.GetRequiredService<ILogger<HyperaiXConfigurationBuilder>>();
            switch (evt)
            {
                case FriendMessageEventArgs it:
                {
                    logger.LogInformation("F | Friend = {FName}({FId})\n{Message}", it.Sender.Nickname,
                        it.Sender.Identity, it.Message);
                    break;
                }
                case GroupMessageEventArgs it:
                {
                    logger.LogInformation("G | Group={GName}({GId}) Member={MName}({MId})\n{Message}", it.Group.Name,
                        it.Group.Identity, it.Sender.DisplayName, it.Sender.Identity, it.Message);
                    break;
                }
                case GroupJoinRequestEventArgs it:
                {
                    logger.LogInformation("{User} requests joining {GroupName}({GroupId}): {Message}", it.UserId,
                        it.Group.Name, it.Group.Identity, it.Message);
                    break;
                }
                case FriendPokeEventArgs it:
                {
                    logger.LogInformation("{UserName}({UserId}) pokes you", it.Sender.Nickname, it.Sender.Identity);
                    break;
                }
                case UnknownEventArgs it:
                {
                    logger.LogInformation("Event blocked, this line is unreachable");
                    break;
                }
            }

            nxt(evt, pvd);
        });
    }
}