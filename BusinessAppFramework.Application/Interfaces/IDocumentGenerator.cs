using BusinessAppFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IDocumentGenerator
   {
      Task<string> GetHtmlDocumentAsync(string key, int objectId);
      Task<byte[]> GetPdfDocumentAsync(string key, int objectId);
   }
}
