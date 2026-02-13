using BusinessAppFramework.Domain;

namespace BusinessAppFramework.DocumentRendering
{
   public interface IHtmlDocumentRenderer
   {
      string RenderDocument(string htmlTemplate, List<object> models);
      Task<byte[]> RenderPdfDocumentAsync(string htmlContent);
   }
}
