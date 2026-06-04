using BusinessAppFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.DocumentRendering
{
   public interface IDocumentModelLoaderTypeRegistry
   {
      void RegisterDocumentModelLoaderType(string key, Type documentModelLoaderType);
      Type GetDocumentModelLoaderType(string key);
   }
}
