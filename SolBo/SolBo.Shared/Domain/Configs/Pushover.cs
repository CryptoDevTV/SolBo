namespace SolBo.Shared.Domain.Configs
{
    public class Pushover
    {
        public string Token { get; set; }
        public string Recipients { get; set; }
        public string Endpoint { get; set; }

        public bool IsActive
            => !string.IsNullOrWhiteSpace(Token) && !string.IsNullOrWhiteSpace(Recipients);
    }
}