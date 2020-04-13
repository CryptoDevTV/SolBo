using System.Collections.Generic;

namespace SolBo.Shared.Domain.Configs
{
    public class App
    {
        public string Name { get; set; }
        public IEnumerable<Exchange> Exchanges { get; set; }
        public Strategy Strategy { get; set; }
    }
}