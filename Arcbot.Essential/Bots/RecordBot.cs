using Arcbot.Essential.Models;
using Arcbot.Essential.Services;
using Hyperai.Events;
using Hyperai.Relations;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arcbot.Essential.Bots
{
    public class RecordBot : BotBase
    {
        private readonly RecordService _service;

        public RecordBot(RecordService service)
        {
            _service = service;
        }

        public override void OnGroupMessage(object sender, GroupMessageEventArgs args)
        {
            if (args.Group.Retrieve(() => new GroupRecordState()).IsOn)
            {
                _service.Push(args.User, args.Message, args.Time);
            }
        }
    }
}
