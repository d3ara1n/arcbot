using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Messages.ConcreteModels;
using Hyperai.Messages.ConcreteModels.FileSources;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SauceNET;

namespace Arcbot.Essential.Units
{
    public class SauceNAOUnit : UnitBase
    {
        [Receive(MessageEventType.Group)]
        [Extract("[hyper.quote({id})][hyper.at({user})] !sauce", true)]
        [Description("用 SauceNAO 搜索图片出处")]
        public async Task GetSauce(Group group, MessageChain raw, Member sender)
        {
            ImageBase img = null;
            if (raw.Any(x => x is Quote))
            {
                var source = await raw.OfMessageRepliedByAsync();
                img = (ImageBase) source.FirstOrDefault(x => x is ImageBase);
            }

            if (img != null && img.Source is UrlSource)
            {
                await group.SendPlainAsync("在找了在找了😚");

                SauceNETClient client = new("653f4b7eb41179c20dc473f3500005fbad22ab3e");
                try
                {
                    var results = await client.GetSauceAsync(((UrlSource) img.Source).Url.AbsoluteUri, "999", "3");
                    var builder = raw.CanBeReplied() ? raw.MakeReply() : new MessageChainBuilder();
                    builder.Add(new At(sender.Identity));
                    builder.AddPlain("来啦来啦😘");

                    foreach (var result in results.Results)
                    {
                        builder.AddImage(null, new UrlSource(new Uri(result.ThumbnailURL, UriKind.Absolute)));
                        builder.AddPlain($"{result.Name}[{result.Similarity}]\n{result.SourceURL}\n");
                    }
                    
                    await group.SendAsync(builder.Build());
                }
                catch
                {
                    await group.SendPlainAsync("出错了出错了😫");
                }
            }
            else
            {
                await group.SendPlainAsync("木有图片😥");
            }
        }
    }
}