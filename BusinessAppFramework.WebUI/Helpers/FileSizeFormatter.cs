using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Helpers
{
   public static class FileSizeFormatter
   {
      public static string ToFriendlySize(long bytes)
      {
         if (bytes < 1024)
            return $"{bytes} B";

         double kb = bytes / 1024d;
         if (kb < 1024)
            return $"{kb:0.#} KB";

         double mb = kb / 1024d;
         if (mb < 1024)
            return $"{mb:0.#} MB";

         double gb = mb / 1024d;
         return $"{gb:0.#} GB";
      }
   }
}
