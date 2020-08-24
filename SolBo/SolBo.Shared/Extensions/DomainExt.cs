using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using System;

namespace SolBo.Shared.Extensions
{
    public static class DomainExt
    {
        public static decimal BoughtPrice(this Solbot solbot)
            => solbot.Strategy.AvailableStrategy.SellType == SellType.FROM_AVERAGE_VALUE
                ? solbot.Communication.Average.Current
                : solbot.Actions.BoughtPrice;

        public static string SellChange(this Solbot solbot)
            => solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{Math.Abs(solbot.Communication.Sell.Change)}"
                : $"{Math.Abs(solbot.Communication.Sell.Change)}%";

        public static string StopLossChange(this Solbot solbot)
            => solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{Math.Abs(solbot.Communication.StopLoss.Change)}"
                : $"{Math.Abs(solbot.Communication.StopLoss.Change)}%";

        public static string BuyChange(this Solbot solbot)
            => solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{solbot.Communication.Buy.Change}"
                : $"{Math.Abs(solbot.Communication.Buy.Change)}%";
    }
}