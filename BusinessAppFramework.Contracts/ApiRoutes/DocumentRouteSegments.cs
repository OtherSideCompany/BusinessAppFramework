using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts.ApiRoutes
{
    public static class DocumentRouteSegments
    {
        public const string Upload = $"upload";
        public const string Delete = $"delete";
        public const string Exists = $"exists";
        public const string GetDownloadUrl = $"get-download-url";
        public const string GetHtml = $"html";
        public const string DownloadPdf = $"download-pdf";
    }
}
