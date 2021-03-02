using System;
using System.Collections.Generic;
using System.Text;

namespace Solbo.Strategy.Beta.Models
{
    public class StrategyCommunicationModel
    {
        public decimal? CurrentPrice { get; set; }
        public string SymbolParsed { get; set; }
    }
}