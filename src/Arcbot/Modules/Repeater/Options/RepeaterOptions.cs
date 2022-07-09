using System.Collections.Generic;

namespace Arcbot.Modules.Repeater.Options;

public class RepeaterOptions
{
    public bool Enabled { get; set; }
    public IEnumerable<long> ActivatedGroups { get; set; }
}