using OtherSideCore.Adapter.Attributes;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectViewModels
{
   public class UserViewModel : DomainObjectViewModel
   {
      #region Fields

      private string _userName;

      #endregion

      #region Properties

      [MonitoredProperty, StringLength(50), EditorLabel("Nom d'utilisateur"), EditorIndex(0)]
      public string UserName
      {
         get => _userName;
         set => SetProperty(ref _userName, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserViewModel(
         DomainObject domainObject, 
         DomainObjectViewModelDependencies domainObjectViewModelDependencies) : 
         base(domainObject, domainObjectViewModelDependencies)
      {
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion

   }
}
