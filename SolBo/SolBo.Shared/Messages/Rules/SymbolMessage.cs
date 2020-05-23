using System.Text.Json;

namespace SolBo.Shared.Messages.Rules
{
    public class SymbolMessage
    {
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
    }
}