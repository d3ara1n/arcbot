using System;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;

namespace Arcbot.Modules.Github.Units;

public class GithubUnit : UnitBase
{
    [Receiver(MessageEventType.Group | MessageEventType.Friend)]
    [Regex(@"^http[s]?:\/\/github.com\/(?<owner>[a-zA-Z0-9_]+)\/(?<repo>[a-zA-Z0-9_]+)$")]
    public Image Url(string owner, string repo)
    {
        return new Image(new Uri($"https://opengraph.githubassets.com/0/{owner}/{repo}"));
    }

    [Receiver(MessageEventType.Group | MessageEventType.Friend)]
    [Regex(@"^(?<owner>[a-zA-Z0-9_]+)\/(?<repo>[a-zA-Z0-9_]+)$")]
    public Image Repo(string owner, string repo)
    {
        return new Image(new Uri($"https://opengraph.githubassets.com/0/{owner}/{repo}"));
    }
}