using OtherSideCore.Application.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Printers
{
   public class PrintersService : IPrintersService
   {
      public List<string> GetAvailablePrinterNames()
      {
         List<string> printerNames = new List<string>();

         foreach (string printerName in PrinterSettings.InstalledPrinters)
         {
            printerNames.Add(printerName.ToString());
         }

         return printerNames;
      }

      public string GetDefaultPrinterName()
      {
         PrinterSettings settings = new PrinterSettings();
         return settings.PrinterName;
      }
   }
}
