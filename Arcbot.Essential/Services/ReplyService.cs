using System;
using System.Collections.Generic;
using Arcbot.Essential.Models;
using Hyperai.Messages;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.ModelExtensions;
using HyperaiShell.Foundation.Plugins;

namespace Arcbot.Essential.Services
{
    public class ReplyService
    {
        private readonly IRepository _repository;

        public ReplyService(IPluginRepository<PluginEntry> repository)
        {
            _repository = repository.Value;
        }

        public void Add(long groupId, long operatorId, MessageChain trigger, MessageChain reply)
        {
            var piece = new GroupReplyPiece()
            {
                CreatedAt = DateTime.Now,
                GroupId = groupId,
                OperatorId = operatorId,
                Trigger = trigger.Flatten(),
                Reply = reply.Flatten()
            };
            _repository.Store(piece);
        }

        public void Remove(int id)
        {
            _repository.Delete<GroupReplyPiece>(id);
        }

        public IEnumerable<GroupReplyPiece> List(long groupId)
        {
            var pieces = _repository.Query<GroupReplyPiece>().Where(x => x.GroupId == groupId);
            return pieces.ToEnumerable();
        }

        public GroupReplyPiece Get(MessageChain chain, long groupId)
        {
            var msg = chain.Flatten();
            return _repository.Query<GroupReplyPiece>().Where(x => x.Trigger == msg && x.GroupId == groupId).FirstOrDefault();
        }
    }
}