namespace BusinessAppFramework.Application.Kpis
{    public class TargetValueKpi
    {
        public decimal CurrentValue { get; set; }

        public decimal? TargetValue { get; set; }

        public bool IsTargetSet => TargetValue != null;

        public decimal Delta => IsTargetSet ? CurrentValue - TargetValue!.Value : 0;

        public bool IsTargetReached => IsTargetSet && CurrentValue >= TargetValue!.Value;

        public decimal ProgressRatio => IsTargetSet && TargetValue!.Value > 0
            ? Math.Clamp(CurrentValue / TargetValue.Value, 0m, 1m)
            : 0m;

        public static TargetValueKpi Build(decimal currentValue, decimal target)
        {
            return new TargetValueKpi
            {
                CurrentValue = currentValue,
                TargetValue = target > 0 ? target : null
            };
        }
    }
}
