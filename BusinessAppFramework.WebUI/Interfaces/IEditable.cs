namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IEditable
   {
      Task SaveChangesAsync();
      Task CancelChangesAsync();
   }
}
