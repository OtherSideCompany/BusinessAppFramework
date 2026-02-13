using PuppeteerSharp;
using PuppeteerSharp.Media;
using Scriban;
using Scriban.Runtime;

namespace BusinessAppFramework.DocumentRendering
{
   public class HtmlDocumentRenderer : IHtmlDocumentRenderer
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public HtmlDocumentRenderer()
      {

      }

      #endregion

      #region Public Methods

      public string RenderDocument(string htmlTemplate, List<object> models)
      {
         var template = TryParseTemplate(htmlTemplate);

         var globalScriptObject = new ScriptObject();

         foreach (var model in models)
         {
            globalScriptObject.Import(model);
         }

         var ctx = new TemplateContext();
         ctx.PushGlobal(globalScriptObject);

         return template.Render(ctx);
      }

      public async Task<byte[]> RenderPdfDocumentAsync(string htmlContent)
      {
         var fetcher = new BrowserFetcher();
         await fetcher.DownloadAsync();

         using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
         using var page = await browser.NewPageAsync();

         await page.EmulateMediaTypeAsync(MediaType.Print);

         await page.SetContentAsync(htmlContent);

         return await page.PdfDataAsync(new PdfOptions
         {
            PreferCSSPageSize = true,
            PrintBackground = true
         });
      }

      #endregion

      #region Private Methods        

      private Template TryParseTemplate(string templateString)
      {
         var template = Template.Parse(templateString);

         if (template.HasErrors)
         {
            throw new InvalidOperationException("Erreur de parsing Scriban : " + string.Join(", ", template.Messages.Select(m => m.Message)));
         }

         return template;
      }

      #endregion
   }
}
