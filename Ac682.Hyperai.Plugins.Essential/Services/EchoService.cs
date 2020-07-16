using Ac682.Hyperai.Plugins.Essential.Models;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.Plugins;

namespace Ac682.Hyperai.Plugins.Essential.Services
{
    public class EchoService
    {
        private readonly IRepository _repository;

        public EchoService(IPluginRepository<PluginEntry> repository)
        {
            _repository = repository.Value;
        }
        public void On(long num)
        {
            EchoTrack track = _repository.Query<EchoTrack>().Where(x => x.Target == num).FirstOrDefault();
            if (track != null)
            {
                track.State = true;
                _repository.Update(track);
            }
            else
            {
                _repository.Store(new EchoTrack() { State = true, Target = num });
            }
        }

        public void Off(long num)
        {
            EchoTrack track = _repository.Query<EchoTrack>().Where(x => x.Target == num).FirstOrDefault();
            if (track != null)
            {
                track.State = false;
                _repository.Update(track);
            }
            else
            {
                _repository.Store(new EchoTrack() { State = false, Target = num });
            }
        }

        public bool IsOn(long num)
        {
            return _repository.Query<EchoTrack>().Where(x => x.Target == num).FirstOrDefault()?.State ?? false;
        }
    }
}
