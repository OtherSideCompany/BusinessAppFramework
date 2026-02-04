namespace Application.Services
{
   public interface IPrintersService
   {
      List<string> GetAvailablePrinterNames();

      string GetDefaultPrinterName();
   }
}
