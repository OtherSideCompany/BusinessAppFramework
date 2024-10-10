using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
    public class ConstraintViewModel<T> : ObservableObject where T : DomainObject, new()
   {
      #region Fields

      private bool _isSelected;
      private string _name;
      private Constraint<T> _constraint;

      #endregion

      #region Properties

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public string Name
      {
         get => _name;
         set => SetProperty(ref _name, value);
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

      public ConstraintViewModel(string name, Constraint<T> constraint)
      {
         Name = name;
         Constraint = constraint;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
