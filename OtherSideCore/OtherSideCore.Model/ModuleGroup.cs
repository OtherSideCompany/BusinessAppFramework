using System.Collections.ObjectModel;
using System.Linq;

namespace OtherSideCore.Model
{
   public class ModuleGroup : ModuleBase
   {
      #region Fields

      private ObservableCollection<Module> m_Modules;

      #endregion

      #region Properties

      public ObservableCollection<Module> Modules
      {
         get => m_Modules;
         set => SetProperty(ref m_Modules, value);
      }

      public bool IsSubModuleLoaded
      {
         get
         {
            return Modules.Any(m => m.IsLoaded);
         }
      }


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleGroup(string name) : base(name)
      {
         Modules = new ObservableCollection<Module>();
         ModuleNavigationPath = Name;

         Modules.CollectionChanged += Modules_CollectionChanged;
      }

      #endregion

      #region Methods

      private void Modules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
      {
         if (e.OldItems != null)
         {
            foreach (var oldItem in e.OldItems)
            {
               (oldItem as Module).PropertyChanged -= Module_PropertyChanged;
            }
         }

         if (e.NewItems != null)
         {
            foreach (var newItem in e.NewItems)
            {
               (newItem as Module).PropertyChanged += Module_PropertyChanged;
            }
         }

         OnPropertyChanged(nameof(IsSubModuleLoaded));
      }

      private void Module_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(IsLoaded)))
         {
            OnPropertyChanged(nameof(IsSubModuleLoaded));
         }
      }

      public override void Dispose()
      {
         base.Dispose();

         Modules.CollectionChanged -= Modules_CollectionChanged;

         foreach (var module in Modules)
         {
            module.PropertyChanged -= Module_PropertyChanged;
         }
      }

      #endregion
   }
}
