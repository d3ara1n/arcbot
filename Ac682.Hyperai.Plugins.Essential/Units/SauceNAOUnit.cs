using Hyperai.Units;
using System.Linq;
using Hyperai.Units.Attributes;
using Hyperai.Events;
using HyperaiShell.Foundation.Authorization.Attributes;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Messages.ConcreteModels;
using System.IO;
using System;
using HyperaiShell.Foundation.ModelExtensions;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ac682.Hyperai.Plugins.Essential.Units
{
    public class SauceNAOUnit : UnitBase
    {
        private readonly ILogger _logger;
        private readonly IMessageChainFormatter _formatter;

        public SauceNAOUnit(ILogger<SauceNAOUnit> logger, IMessageChainFormatter formatter)
        {
            _logger = logger;
            _formatter = formatter;
        }

        [Receive(MessageEventType.Group)]
        [Extract("!sauce {image}")]
        [CheckTicket("sauce.get")]
        public async Task GetSauce(MessageChain image, Group group)
        {
            var img = (Image)image.FirstOrDefault(x => x is Image);
            _logger.LogInformation("Processing {}...", image.ToString());
            if (img == null)
            {
                await group.SendPlainAsync("No image.");
                return;
            }
            try
            {
                string tmp = Path.GetTempFileName();

                using (var reader = img.OpenRead())
                {
                    using FileStream writer = new FileStream(tmp, FileMode.Append);
                    await reader.CopyToAsync(writer);
                }
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://saucenao.com/");
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(new FileStream(tmp, FileMode.Open)), "file", img.Url.AbsoluteUri);
                var response = await client.PostAsync("search.php?output_type=2", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var builder = image.CanBeReplied() ? image.MakeReply() : new MessageChainBuilder();
                    builder.AddPlain("Result: \n");
                    var obj = JsonConvert.DeserializeObject<JObject>(json);
                    //builder.Add(img);
                    foreach (var result in obj.Value<JArray>("results"))
                    {
                        var thumbnail = result["header"].Value<string>("thumbnail");
                        var similarity = result["header"].Value<string>("similarity");
                        var url = result["data"]["ext_urls"].Values<string>().FirstOrDefault() ?? "[UNKNOWN]";
                        var title = result["data"].Value<string>("title") ?? "[UNKNOWN]";
                        var member = result["data"].Value<string>("member_name");
                        builder.AddImage(new Uri(thumbnail));
                        builder.AddPlain($"[{similarity}%]({title} - {member}: {url})\n");
                    }
                    var msg = builder.Build();
                    _logger.LogDebug(_formatter.Format(msg));
                    await group.SendAsync(msg);
                }
                else
                {
                    await group.SendPlainAsync($"Unexpected code {response.StatusCode} returned.");
                    return;
                }
            }
            catch (Exception e)
            {
                await group.SendPlainAsync($"Exception occurred: {e}");
            }
        }
    }
}