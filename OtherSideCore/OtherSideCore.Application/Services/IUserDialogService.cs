namespace OtherSideCore.Appplication.Services
{
   public interface IUserDialogService
   {
      bool Confirm(string message);
      void Error(string message);
   }
}
