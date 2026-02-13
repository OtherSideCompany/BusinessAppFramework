using BusinessAppFramework.Adapter.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IDocumentGeneratorGateway
   {
      Task<DocumentHtmlResponse?> GetHtmlDocumentAsync(string key, int objectId);
   }
}
