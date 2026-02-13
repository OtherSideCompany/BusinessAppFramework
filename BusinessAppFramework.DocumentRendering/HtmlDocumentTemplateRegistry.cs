using BusinessAppFramework.Application.Registry;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.DocumentRendering
{
    public class HtmlDocumentTemplateRegistry : Registry<StringKey, HtmlDocumentTemplate>, IHtmlDocumentTemplateRegistry
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public HtmlDocumentTemplateRegistry()
        {

        }

        #endregion

        #region Public Methods

        public void RegisterTemplate(StringKey key, HtmlDocumentTemplate template)
        {
            Register(key, template);
        }

        public HtmlDocumentTemplate GetTemplate(StringKey key)
        {
            return Resolve(key);
        }               

        #endregion

        #region Private Methods



        #endregion
    }
}
