using BusinessAppFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.DocumentRendering
{
   public interface IDocumentModelLoaderTypeRegistry
   {
      void RegisterDocumentModelLoaderType(StringKey key, Type documentModelLoaderType);
      Type GetDocumentModelLoaderType(StringKey key);
   }
}
