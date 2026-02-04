using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
    public class ConstraintViewModel<TSearchResult> : ObservableObject where TSearchResult : DomainObjectSearchResult, new()
   {
      #region Fields

      private bool _isSelected;
      private Constraint<TSearchResult> _constraint;

      #endregion

      #region Properties

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public Constraint<TSearchResult> Constraint
      {
         get => _constraint;
         private set => SetProperty(ref _constraint, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ConstraintViewModel(Constraint<TSearchResult> constraint)
      {
         Constraint = constraint;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
