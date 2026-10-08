using PuppeteerSharp;
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

        private readonly string _executablePath;
        private readonly string[] _launchArguments;

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

        public HtmlDocumentRenderer(string? executablePath = null, string[]? launchArguments = null)
        {
            _executablePath = string.IsNullOrWhiteSpace(executablePath)
                ? Path.Combine(AppContext.BaseDirectory, "chrome", "chrome.exe")
                : executablePath;

            _launchArguments = launchArguments ?? [];
        }

        #endregion

        #region Public Methods

        public string RenderDocument(string htmlTemplate, List<object> models)
        {
            var template = TryParseTemplate(htmlTemplate);

            var globalScriptObject = new ScriptObject();

            foreach (var model in models)
            {
                ImportModel(globalScriptObject, model);
            }

            var ctx = new TemplateContext();
            ctx.PushGlobal(globalScriptObject);

            return template.Render(ctx);
        }

        public async Task<byte[]> RenderPdfDocumentAsync(string htmlContent, PdfPageLayout? pageLayout = null)
        {
            var browser = await GetBrowserAsync();

            await using var page = await browser.NewPageAsync();

            await page.EmulateMediaTypeAsync(MediaType.Print);
            await page.SetContentAsync(htmlContent, new SetContentOptions
            {
                WaitUntil = [WaitUntilNavigation.Networkidle0]
            });

            return await page.PdfDataAsync(CreatePdfOptions(pageLayout));
        }

        #endregion

        #region Private Methods

        private static void ImportModel(ScriptObject globalScriptObject, object model)
        {
            switch (model)
            {
                case Type functionsType:
                    globalScriptObject.Import(functionsType);
                    break;
                case IScriptObject scriptObject:
                    globalScriptObject.Import(scriptObject);
                    break;
                default:
                    globalScriptObject.Import(model);
                    break;
            }
        }

        private static PdfOptions CreatePdfOptions(PdfPageLayout? pageLayout)
        {
            var options = new PdfOptions
            {
                PreferCSSPageSize = true,
                PrintBackground = true
            };

            if (pageLayout == null)
            {
                return options;
            }

            options.MarginOptions = new MarginOptions
            {
                Top = pageLayout.MarginTop,
                Bottom = pageLayout.MarginBottom,
                Left = pageLayout.MarginLeft,
                Right = pageLayout.MarginRight
            };

            if (pageLayout.FooterTemplate != null)
            {
                options.DisplayHeaderFooter = true;
                options.HeaderTemplate = "<span></span>";
                options.FooterTemplate = pageLayout.FooterTemplate;
            }

            return options;
        }

        private async Task<IBrowser> GetBrowserAsync()
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

                if (!File.Exists(_executablePath))
                {
                    throw new FileNotFoundException(
                        $"Chrome was not found at '{_executablePath}'. Download it with 'dotnet run --project Cyklor.ChromePackager -- download <directory>' " +
                        "and set DocumentRendering:ChromeExecutablePath to the resulting chrome.exe.",
                        _executablePath);
                }

                _browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = _executablePath,
                    Args = _launchArguments
                });

                return _browser;
            }
            finally
            {
                _browserSemaphore.Release();
            }
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
