using Onebot.Protocol.Models.Messages;
using Onebot.Protocol.Models.Receipts;

namespace Arcbot.Models.Receipts;

public record GetMessageReceipt : ReceiptBase
{
    public string MessageId { get; set; }
    public Message Message { get; set; }
    public string AltMessage { get; set; }
    public string UserId { get; set; }
    public string GroupId { get; set; }
    public string UserName { get; set; }
}