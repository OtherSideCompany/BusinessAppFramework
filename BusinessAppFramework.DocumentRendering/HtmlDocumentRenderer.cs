using PuppeteerSharp;
using PuppeteerSharp.BrowserData;
using PuppeteerSharp.Media;
using Scriban;
using Scriban.Runtime;

namespace BusinessAppFramework.DocumentRendering
{
    public class HtmlDocumentRenderer : IHtmlDocumentRenderer
    {
        #region Fields

        private static readonly SemaphoreSlim _browserSemaphore = new(1, 1);
        private static IBrowser? _browser;
        private static bool _browserDownloaded = false;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        static HtmlDocumentRenderer()
        {
            AppDomain.CurrentDomain.ProcessExit += (_, _) => ShutdownBrowser();
        }

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
            var browser = await GetBrowserAsync();

            await using var page = await browser.NewPageAsync();

            await page.EmulateMediaTypeAsync(MediaType.Print);
            await page.SetContentAsync(htmlContent, new SetContentOptions
            {
                WaitUntil = [WaitUntilNavigation.Networkidle0]
            });

            return await page.PdfDataAsync(new PdfOptions
            {
                PreferCSSPageSize = true,
                PrintBackground = true
            });
        }

        #endregion

        #region Private Methods        

        private static async Task<IBrowser> GetBrowserAsync()
        {
            if (_browser is { IsClosed: false })
            {
                return _browser;
            }

            await _browserSemaphore.WaitAsync();
            try
            {
                if (_browser is { IsClosed: false })
                {
                    return _browser;
                }

                if (_browser is not null)
                {
                    await _browser.DisposeAsync();
                    _browser = null;
                }

                await EnsureBrowserDownloadedAsync();

                _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

                return _browser;
            }
            finally
            {
                _browserSemaphore.Release();
            }
        }

        private static async Task EnsureBrowserDownloadedAsync()
        {
            if (_browserDownloaded)
            {
                return;
            }

            var fetcher = new BrowserFetcher();
            var buildId = Chrome.DefaultBuildId;

            if (fetcher.GetInstalledBrowsers().All(b => b.BuildId != buildId))
            {
                await fetcher.DownloadAsync(buildId);
            }

            _browserDownloaded = true;
        }

        private static void ShutdownBrowser()
        {
            var browser = Interlocked.Exchange(ref _browser, null);

            if (browser is null)
            {
                return;
            }

            try
            {
                browser.CloseAsync().GetAwaiter().GetResult();
                browser.Dispose();
            }
            catch
            {
                // Nothing useful to do while the process is exiting.
            }
        }

        private Template TryParseTemplate(string templateString)
        {
            var template = Template.Parse(templateString);

            if (template.HasErrors)
            {
                throw new InvalidOperationException("Scriban parsing error : " + string.Join(", ", template.Messages.Select(m => m.Message)));
            }

            return template;
        }

        #endregion
    }
}
