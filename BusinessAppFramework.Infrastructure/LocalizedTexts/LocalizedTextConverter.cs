using AutoMapper;
using BusinessAppFramework.Domain.LocalizedTexts;

namespace BusinessAppFramework.Infrastructure.LocalizedTexts
{
    public class LocalizedTextConverter :
    ITypeConverter<string, Domain.LocalizedTexts.LocalizedText>,
    ITypeConverter<Domain.LocalizedTexts.LocalizedText, string>
    {
        public Domain.LocalizedTexts.LocalizedText Convert(string source, Domain.LocalizedTexts.LocalizedText destination, ResolutionContext context) =>
            LocalizedTextJson.Deserialize(source);

        public string Convert(Domain.LocalizedTexts.LocalizedText source, string destination, ResolutionContext context) =>
            LocalizedTextJson.Serialize(source);
    }
}
