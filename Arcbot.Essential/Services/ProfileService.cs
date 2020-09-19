using System;
using System.Collections.Generic;
using System.Linq;
using Arcbot.Essential.Models;
using Arcbot.Essential.Models.ProfileInventory;
using Hyperai.Relations;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.Logging;

namespace Arcbot.Essential.Services
{
    public class ProfileService
    {
        private readonly IRepository _repository;

        public ProfileService(IPluginRepository<PluginEntry> repository)
        {
            _repository = repository.Value;
        }

        public long Put(User user, Item what)
        {
            var profile = _repository.Query<Profile>().Where(x => x.UserAttachedTo == user.Identity).FirstOrDefault() ?? new Profile(user.Identity);
            var stack = profile.Inventory.FirstOrDefault(x => x.GetType() == what.GetType());
            if (stack != null)
            {
                stack.Stack += what.Stack;
                stack.LastModified = DateTime.Now;
            }
            else
            {
                profile.Inventory.Add(what);
                what.CreatedAt = what.LastModified = DateTime.Now;
            }
            _repository.Upsert(profile);
            return stack?.Stack ?? what.Stack;
        }

        public long Take(User user, Item what)
        {
            var profile = _repository.Query<Profile>().Where(x => x.UserAttachedTo == user.Identity).FirstOrDefault() ?? new Profile(user.Identity);
            var stack = profile.Inventory.FirstOrDefault(x => x.GetType() == what.GetType());
            if (stack != null)
            {
                stack.Stack -= what.Stack;
                stack.LastModified = DateTime.Now;
            }
            else
            {
                profile.Inventory.Add(what);
                what.CreatedAt = what.LastModified = DateTime.Now;
            }
            _repository.Upsert(profile);
            return stack?.Stack ?? what.Stack;
        }

        public long Count<TItem>(User user)
        where TItem : Item
        {
            var profile = _repository.Query<Profile>().Where(x => x.UserAttachedTo == user.Identity).FirstOrDefault() ?? new Profile(user.Identity);
            var stack = profile.Inventory.FirstOrDefault(x => x.GetType() == typeof(TItem));
            if (stack != null)
            {
                return stack.Stack;
            }
            else
            {
                return 0;
            }
        }

        public TItem Inspect<TItem>(User user)
        where TItem : Item
        {
            var profile = _repository.Query<Profile>().Where(x => x.UserAttachedTo == user.Identity).FirstOrDefault();
            return profile == null ? default : (TItem)profile.Inventory.FirstOrDefault(x => x.GetType() == typeof(TItem));
        }

        public IEnumerable<Item> Items(User user)
        {
            var profile = _repository.Query<Profile>().Where(x => x.UserAttachedTo == user.Identity).FirstOrDefault();
            return profile == null ? Enumerable.Empty<Item>() : profile.Inventory.AsEnumerable();
        }

        public bool Consume(User user, Item what)
        {
            var profile = _repository.Query<Profile>().Where(x => x.UserAttachedTo == user.Identity).FirstOrDefault() ?? new Profile(user.Identity);
            var stack = profile.Inventory.FirstOrDefault(x => x.GetType() == what.GetType());
            if (stack != null && stack.Stack > what.Stack)
            {
                stack.Stack -= what.Stack;
                stack.LastModified = DateTime.Now;
                _repository.Upsert(profile);
                return true;
            }
            else
            {
                return false;
            }
        }

        public long CountCoin(User user) => Count<Coin>(user);
        public long TakeCoin(User user, long howMany) => Take(user, new Coin(howMany));
        public long PutCoin(User user, long howMany) => Put(user, new Coin(howMany));
    }
}