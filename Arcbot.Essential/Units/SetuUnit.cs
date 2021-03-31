using System.ComponentModel;
using Arcbot.Essential.Models;
using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using Wupoo;
using HyperaiShell.Foundation.ModelExtensions;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hyperai.Messages;
using System;
using Microsoft.Extensions.Logging;
using Arcbot.Essential.Services;
using HyperaiShell.Foundation.Authorization.Attributes;
using System.Linq;

namespace Arcbot.Essential.Units
{
    public class SetuUnit : UnitBase
    {
        private readonly Random random = new();
        private readonly WapooOptions options = new();

        private readonly ILogger _logger;
        private readonly ProfileService _profileService;

        public SetuUnit(ILogger<SetuUnit> logger, ProfileService profileService)
        {
            _logger = logger;
            _profileService = profileService;
        }

        [Receive(MessageEventType.Group)]
        [Extract("!setu")]
        [Description("消耗 0~2 个硬币抽取一张随机 setu")]
        public async Task Setu(Group group, Member member)
        {
            const string url = "http://api.yuban10703.xyz:2333/setu_v2";
            int cost = random.Next(3);
            SetuWhite white = group.Retrieve(() => new SetuWhite());
            if (!white.IsOn)
            {
                if (_profileService.CountCoin(member) < cost)
                {
                    await group.SendPlainAsync("你硬币不够.");
                }
                SetuArtwork artwork = null;
                var task1 = group.SendPlainAsync("来了来了😜");
                var task2 = Request(url)
                .ForJsonResult<JObject>(obj =>
                {
                    try
                    {
                        artwork = obj.Value<JArray>("data").FirstOrDefault()?.ToObject<SetuArtwork>();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Fetching setu error.");
                    }
                })
                .FetchAsync();
                await task1;
                await task2;
                if (artwork != null)
                {
                    var builder = new MessageChainBuilder();
                    builder.AddImage(new Uri(artwork.Original.Replace("pximg.net", "pixiv.cat"), UriKind.Absolute));
                    builder.AddPlain($"[Pixiv]\nArtwork: {artwork.Title}({artwork.Artwork})\nAuthor: {artwork.Author}\nCost: {cost}💰\nUrl: {artwork.Original}\nTags: {string.Join(',', artwork.Tags)}");
                    _profileService.TakeCoin(member, cost);
                    await group.SendAsync(builder.Build());
                }
                else
                {
                    await group.SendPlainAsync("出错力😥");
                }
            }

        }

        [Receive(MessageEventType.Group)]
        [Extract("!setu.on")]
        [Description("允许本群群员请求 setu")]
        [CheckTicket("setu.control")]
        public async Task On(Group group)
        {
            group.For(out SetuWhite white, () => new SetuWhite(true));
            white.IsOn = true;
            await group.SendPlainAsync("车道铺好了.");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!setu.off")]
        [Description("禁止本群群员请求 setu")]
        [CheckTicket("setu.control")]
        public async Task Off(Group group)
        {
            group.For(out SetuWhite white, () => new SetuWhite(false));
            white.IsOn = false;
            await group.SendPlainAsync("车道拆掉了.");
        }

        private Wapoo Request(string url)
        {
            return new Wapoo(options, url);
        }
    }
}