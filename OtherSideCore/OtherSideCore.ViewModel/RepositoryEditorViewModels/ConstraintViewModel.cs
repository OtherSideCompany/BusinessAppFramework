using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel.RepositoryEditorViewModels
{
   public class ConstraintViewModel : ObservableObject
   {
      #region Fields

      private bool _isSelected;
      private Constraint _constraint;

      #endregion

      #region Properties

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public Constraint Constraint
      {
         get => _constraint;
         private set => SetProperty(ref _constraint, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ConstraintViewModel(Constraint constraint)
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
