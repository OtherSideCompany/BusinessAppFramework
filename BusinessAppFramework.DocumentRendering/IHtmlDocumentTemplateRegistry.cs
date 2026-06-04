using BusinessAppFramework.Domain;

namespace BusinessAppFramework.DocumentRendering
{
    public interface IHtmlDocumentTemplateRegistry
    {
        void RegisterTemplate(string key, HtmlDocumentTemplate template);
        HtmlDocumentTemplate GetTemplate(string key);
    }
}
