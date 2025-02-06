using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application.Search;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectSearchResultViewModel : ObservableObject, IDisposable, ISelectable
   {
      #region Fields

      private DomainObjectSearchResult _domainObjectSearchResult;

      private bool _isSelected;
      private bool _isExpanded;

      #endregion

      #region Properties

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetProperty(ref _isExpanded, value);
      }

      public DomainObjectSearchResult DomainObjectSearchResult
      {
         get => _domainObjectSearchResult;
         private set => SetProperty(ref _domainObjectSearchResult, value);
      }


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectSearchResultViewModel(DomainObjectSearchResult domainObjectSearchResult)
      {
         DomainObjectSearchResult = domainObjectSearchResult;
      }

      #endregion

      #region Public Methods

      public virtual void UpdateDomainObjectSearchResult(DomainObjectSearchResult domainObjectSearchResult)
      {
         DomainObjectSearchResult = domainObjectSearchResult;
      }

      public virtual void Dispose()
      {

      }

      #endregion

      #region Private Methods



      #endregion
   }
}
