using System.Collections.Generic;

namespace SolBo.Shared.Domain.Configs
{
    public class App
    {
        public string Version { get; set; }
        public IEnumerable<Strategy> Strategies { get; set; }
        public Notifications Notifications { get; set; }
        public Exchange Exchange { get; set; }
    }
}