using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public interface ISavable : INotifyPropertyChanged
   {
      bool HasUnsavedChanges { get; }
      bool CanSaveChanges();
      Task SaveChangesAsync();
      bool CanCancelChanges();
      Task CancelChangesAsync();

   }
}
