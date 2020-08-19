namespace SolBo.Shared.Rules
{
    public interface IOrderRule : IRule
    {
        string OrderStep { get; }
    }
}