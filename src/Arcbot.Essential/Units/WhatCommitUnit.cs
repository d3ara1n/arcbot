using System.ComponentModel;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;
using Wupoo;

namespace Arcbot.Essential.Units
{
    public class WhatCommitUnit: UnitBase
    {

        [Receive(MessageEventType.Group)]
        [Description("随机一条 commit message")]
        [Extract("!whatcommit")]
        public async Task CommitMessage(Group group, MessageChain chain)
        {
            string txt = null;
            await Wapoo
                .Wohoo("http://whatthecommit.com/index.txt")
                .ViaGet()
                .ForStringResult(x => txt = x)
                .FetchAsync();

            var builder = chain.CanBeReplied() ? chain.MakeReply() : new MessageChainBuilder();
            builder.AddPlain(txt.Trim());
            await group.SendAsync(builder.Build());
        }
    }
}