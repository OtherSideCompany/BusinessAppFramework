using System;
using System.Collections.Generic;
using System.Reflection;
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

        /// <summary>
        /// Assembly holding the layout resource. When null, the document generator assembly is used.
        /// </summary>
        public Assembly? LayoutAssembly { get; set; }

        /// <summary>
        /// Assembly holding the content resource and its partials. When null, the document generator assembly is used.
        /// </summary>
        public Assembly? ContentAssembly { get; set; }

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

        public HtmlDocumentTemplate(string layoutResourceName, string contentResourceName, Assembly? contentAssembly)
           : this(layoutResourceName, contentResourceName)
        {
            ContentAssembly = contentAssembly;
        }

        public HtmlDocumentTemplate(string layoutResourceName, Assembly? layoutAssembly, string contentResourceName, Assembly? contentAssembly)
           : this(layoutResourceName, contentResourceName)
        {
            LayoutAssembly = layoutAssembly;
            ContentAssembly = contentAssembly;
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
