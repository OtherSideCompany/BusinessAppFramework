namespace WebUI.Components.Selector
{
   public interface IDomainObjectSelector
   {
      Task Load(string filter);
   }
}
