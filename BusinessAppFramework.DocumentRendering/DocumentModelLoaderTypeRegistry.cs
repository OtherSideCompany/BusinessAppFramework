using BusinessAppFramework.Application.Registry;
using BusinessAppFramework.Domain;
using Scriban;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.DocumentRendering
{
   public class DocumentModelLoaderTypeRegistry : Registry<string, Type>, IDocumentModelLoaderTypeRegistry
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DocumentModelLoaderTypeRegistry()
      {

      }

      #endregion

      #region Public Methods

      public Type GetDocumentModelLoaderType(string key)
      {
         return Resolve(key);
      }

      public void RegisterDocumentModelLoaderType(string key, Type documentModelLoaderType)
      {
         Register(key, documentModelLoaderType);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
