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
            int cost = random.Next(3);
            SetuWhite white = group.Retrieve(() => new SetuWhite());
            string url = white.SexyMode ? "http://api.yuban10703.xyz:2333/setu_v2" : "http://api.yuban10703.xyz:2333/setu_v4";
            _logger.LogInformation("{groupName}({groupId}) requests one setu. (IsOn = {isOn}, SexyMode = {sexyMode}).", group.Name, group.Identity, white.IsOn, white.SexyMode);
            if (white.IsOn)
            {
                if (_profileService.CountCoin(member) < cost)
                {
                    await group.SendPlainAsync("你硬币不够.");
                    return;
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
                    builder.AddPlain($"\n[Pixiv]\nArtwork: {artwork.Title}({artwork.Artwork})\nAuthor: {artwork.Author}\nCost: {cost}💰\nUrl: {artwork.Original}\nTags: {string.Join(',', artwork.Tags)}");
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
            using (group.For(out SetuWhite white, () => new SetuWhite(true)))
            {
                white.IsOn = true;
                await group.SendPlainAsync("车道铺好了.");
            }
        }

        [Receive(MessageEventType.Group)]
        [Extract("!setu.off")]
        [Description("禁止本群群员请求 setu")]
        [CheckTicket("setu.control")]
        public async Task Off(Group group)
        {
            using (group.For(out SetuWhite white, () => new SetuWhite(false)))
            {
                white.IsOn = false;
                await group.SendPlainAsync("车道拆掉了.");
            }
        }

        [Receive(MessageEventType.Group)]
        [Extract("!setu.sexy {mode}")]
        [Description("改变群setu模式，开启sexy或关闭 (on/off/show)")]
        [CheckTicket("setu.control")]
        public async Task Sexy(Group group, string mode)
        {
            using (group.For(out SetuWhite white, () => new SetuWhite()))
            {
                switch (mode)
                {
                    case "on":
                        white.SexyMode = true;
                        await group.SendPlainAsync("注意道路安全！");
                        break;
                    case "off":
                        white.SexyMode = false;
                        await group.SendPlainAsync("正在前往：幼儿园！");
                        break;
                    case "show":
                        await group.SendPlainAsync($"目前模式: {(white.SexyMode ? "青壮年模式" : "青少年模式")}");
                        break;
                    default:
                        await group.SendPlainAsync("参数就一个单词，on/off/show！");
                        break;
                }
            }
        }

        private Wapoo Request(string url)
        {
            return new Wapoo(options, url);
        }
    }
}