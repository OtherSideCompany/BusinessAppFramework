using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.DocumentRendering
{
    public class HtmlDocumentTemplate
    {
        #region Fields



        #endregion

        #region Properties

        public string LayoutResourceName { get; set; } = default!;
        public string ContentResourceName { get; set; } = default!;

        #endregion

        #region Events



        #endregion

        #region Constructor

        public HtmlDocumentTemplate()
        {

        }

        public HtmlDocumentTemplate(string layoutResourceName, string contentResourceName)
        {
            LayoutResourceName = layoutResourceName;
            ContentResourceName = contentResourceName;
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
