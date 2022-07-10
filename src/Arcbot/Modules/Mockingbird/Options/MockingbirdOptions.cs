using System.Collections.Generic;

namespace Arcbot.Modules.Mockingbird.Options;

public class MockingbirdOptions
{
    public bool Enabled { get; set; }
    public IEnumerable<long> ActivatedGroups { get; set; }
}