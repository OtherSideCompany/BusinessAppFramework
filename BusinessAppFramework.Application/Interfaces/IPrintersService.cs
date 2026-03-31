namespace BusinessAppFramework.Application.Interfaces
{
   public interface IPrintersService
   {
      List<string> GetAvailablePrinterNames();

      string GetDefaultPrinterName();
   }
}
