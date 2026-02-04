namespace BusinessAppFramework.Application.Services
{
   public interface IUserDialogService
   {
      Task<bool> ConfirmAsync(string message);
      void Error(string message);
      void Show(string message);
   }
}
