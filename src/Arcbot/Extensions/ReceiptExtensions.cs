using H = HyperaiX.Abstractions.Receipts;
using O = Onebot.Protocol.Models.Receipts;

namespace Arcbot.Extensions
{
    public static class ReceiptExtensions
    {
        public static H.GenericReceipt ToReceipt(this O.IReceipt receipt) => receipt switch
        {
            O.GenericReceipt it => new H.GenericReceipt(),
            O.MessageReceipt it => new H.MessageReceipt()
            {
                MessageId = it.MessageId
            }
        };
    }
}