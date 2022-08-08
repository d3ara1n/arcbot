using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcbot.Data;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.Options;

namespace Arcbot.Modules.Mockingbird.Units;

public class ManagementUnit : UnitBase
{
    private readonly ArcContext _context;
    private readonly ArcbotOptions _options;

    public ManagementUnit(ArcContext context, IOptions<ArcbotOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    [Receiver(MessageEventType.Group)]
    [Extract("!mockingbird.list")]
    public void List(Group group, Member member)
    {
        var triggers = _context.Triggers.Where(x => x.Group == group.Identity).ToList();
        var builder = new StringBuilder($"{group.Name}({group.Identity})\n");
        foreach (var trigger in triggers)
        {
            builder.AppendLine($"[{trigger.Id}]{trigger.Keyword}=>{trigger.Response},");
        }

        Context.SendAsync(MessageChain.Construct(new Plain(builder.ToString().TrimEnd()))).Wait();
    }

    [Receiver(MessageEventType.Group)]
    [Extract("!mockingbird.delete {id}")]
    public void Delete(Group group, Member member, int id)
    {
        if (member.Role == GroupRole.Administrator || member.Identity == _options.Administrator)
        {
            var result = _context.Triggers.Find(id);
            if (result == null)
            {
                Context.SendAsync(MessageChain.Construct(new Plain("Id Not Found"))).Wait();
            }
            else
            {
                _context.Triggers.Remove(result);
                _context.SaveChanges();
                Context.SendAsync(MessageChain.Construct(new Plain($"{result.Keyword} 被删掉了"))).Wait();
            }
        }
    }
}