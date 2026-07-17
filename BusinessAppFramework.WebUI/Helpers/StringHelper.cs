using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BusinessAppFramework.WebUI.Helpers
{
    public static class StringHelper
    {
        private static readonly string[] GroupSuffixes = { "", "k", "m" };
        private const decimal ThousandThreshold = 1000m;

        public static string AmountToFriendlyDisplay(decimal value)
        {
            if (Math.Abs(value) < ThousandThreshold)
            {
                return FormatBelowThousand(value);
            }

            return FormatWithGroupSuffixes(value);
        }

        private static string FormatBelowThousand(decimal value)
        {
            string formatted = value.ToString(CultureInfo.InvariantCulture);
            if (!formatted.Contains('.'))
            {
                return formatted;
            }

            return formatted.TrimEnd('0').TrimEnd('.');
        }

        private static string FormatWithGroupSuffixes(decimal value)
        {
            string sign = value < 0 ? "-" : string.Empty;
            long amount = (long)Math.Abs(decimal.Truncate(value));

            int highestGroupIndex = HighestGroupIndex(amount);
            var builder = new StringBuilder(sign);

            for (int groupIndex = highestGroupIndex; groupIndex >= 1; groupIndex--)
            {
                long groupValue = groupIndex == highestGroupIndex
                    ? LeadingGroupValue(amount, groupIndex)
                    : InnerGroupValue(amount, groupIndex);

                if (groupValue == 0)
                {
                    continue;
                }

                builder.Append(groupValue);
                builder.Append(GroupSuffixes[groupIndex]);
            }

            return builder.ToString();
        }

        private static int HighestGroupIndex(long amount)
        {
            int maxIndex = GroupSuffixes.Length - 1;
            int groupIndex = 0;
            long remaining = amount;

            while (remaining >= 1000 && groupIndex < maxIndex)
            {
                remaining /= 1000;
                groupIndex++;
            }

            return groupIndex;
        }

        private static long LeadingGroupValue(long amount, int groupIndex)
        {
            return amount / Pow1000(groupIndex);
        }

        private static long InnerGroupValue(long amount, int groupIndex)
        {
            return (amount / Pow1000(groupIndex)) % 1000;
        }

        private static long Pow1000(int exponent)
        {
            long result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= 1000;
            }

            return result;
        }
    }
}
