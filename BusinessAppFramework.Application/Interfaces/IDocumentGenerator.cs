using BusinessAppFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IDocumentGenerator
   {
      Task<string> GetHtmlDocumentAsync(StringKey key, int objectId);
      Task<byte[]> GetPdfDocumentAsync(StringKey key, int objectId);
   }
}
