namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectTreeSearchViewModel : IDisposable
   {
      Task SearchAsync(DomainObjectViewModel domainObjectViewModel);
      void LoadSearchResultViewModels();
      void UnloadSearchResultViewModels();
   }
}
