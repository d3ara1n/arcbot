using Ac682.Hyperai.Plugins.Essential.Models;
using Hyperai.Messages;
using Hyperai.Relations;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ac682.Hyperai.Plugins.Essential.Services
{
    public class RecordService
    {
        private readonly IMessageChainFormatter _formatter;
        private readonly IRepository _repository;

        public RecordService(IMessageChainFormatter formatter, IPluginRepository<PluginEntry> repository)
        {
            _formatter = formatter;
            _repository = repository.Value;
        }

        public void Push(Member member, MessageChain message, DateTime time)
        {
            var record = new Record()
            {
                Message = _formatter.Format(message),
                Who = member.Identity,
                Group = member.Group.Value.Identity,
                TimeStamp = (long)(time - new DateTime(1970, 1, 1)).TotalSeconds
            };
            _repository.Store(record);
        }
        public (long, int)[] Rank(Group group, DateTime? date = null)
        {
            long timestamp = date == null ? 0L : (long)(date.Value - new DateTime(1970, 1, 1)).TotalSeconds;
            var records = _repository.Query<Record>().Where(x => x.Group == group.Identity && (date == null || timestamp == x.TimeStamp)).ToEnumerable();
            var dic = new Dictionary<long, int>();
            foreach (var record in records)
            {
                if (dic.ContainsKey(record.Who))
                {
                    dic[record.Who]++;
                }
                else
                {
                    dic.Add(record.Who, 1);
                }
            }
            return dic.Select(x => (x.Key, x.Value)).OrderByDescending(x => x.Value).Take(10).ToArray();
        }
    }
}
