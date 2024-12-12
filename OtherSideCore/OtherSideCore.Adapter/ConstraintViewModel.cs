using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application.Browser;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
    public class ConstraintViewModel<T> : ObservableObject where T : DomainObject, new()
   {
      #region Fields

      private bool _isSelected;
      private Constraint<T> _constraint;

      #endregion

      #region Properties

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public Constraint<T> Constraint
      {
         get => _constraint;
         private set => SetProperty(ref _constraint, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ConstraintViewModel(Constraint<T> constraint)
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
