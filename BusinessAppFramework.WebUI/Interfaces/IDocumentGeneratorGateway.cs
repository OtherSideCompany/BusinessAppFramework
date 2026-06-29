using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.WebUI.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IDocumentGeneratorGateway
   {
      Task<DocumentHtmlResponse?> GetHtmlDocumentAsync(string key, int objectId);
      Task<DocumentDownloadResult?> DownloadPdfAsync(string key, int objectId, CancellationToken cancellationToken = default);
   }
}
