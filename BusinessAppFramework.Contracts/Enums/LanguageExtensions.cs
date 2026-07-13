using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts.Enums
{
    public static class LanguageExtensions
    {
        public static string ToCultureCode(this Language language) => language switch
        {
            Language.fr => "fr",
            Language.en => "en",
            _ => throw new ArgumentOutOfRangeException(nameof(language))
        };
    }
}
