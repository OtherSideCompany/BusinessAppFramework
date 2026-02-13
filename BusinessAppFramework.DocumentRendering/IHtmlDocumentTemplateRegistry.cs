using BusinessAppFramework.Domain;

namespace BusinessAppFramework.DocumentRendering
{
    public interface IHtmlDocumentTemplateRegistry
    {
        void RegisterTemplate(StringKey key, HtmlDocumentTemplate template);
        HtmlDocumentTemplate GetTemplate(StringKey key);
    }
}
