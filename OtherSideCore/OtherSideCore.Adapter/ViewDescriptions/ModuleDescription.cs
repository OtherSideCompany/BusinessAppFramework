using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.Views;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.ViewDescriptions
{
   public class ModuleDescription : ViewDescriptionBase
   {
      #region Fields

      private bool _isExpanded;

      private ObservableCollection<WorkspaceDescription> _subViewDescriptions;

      #endregion

      #region Properties

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetProperty(ref _isExpanded, value);
      }

      public ObservableCollection<WorkspaceDescription> SubViewDescriptions
      {
         get => _subViewDescriptions;
         set => SetProperty(ref _subViewDescriptions, value);
      }

      public bool IsSubViewLoaded
      {
         get
         {
            return SubViewDescriptions.Any(m => m.IsLoaded);
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleDescription(string name, Type viewModelType, object iconResource) : base(name, viewModelType, iconResource)
      {
         SubViewDescriptions = new ObservableCollection<WorkspaceDescription>();

         ViewNavigationPath = Name;

         SubViewDescriptions.CollectionChanged += SubViews_CollectionChanged;
      }

      #endregion

      #region Public Methods  

      public override void Dispose()
      {
         base.Dispose();

         SubViewDescriptions.CollectionChanged -= SubViews_CollectionChanged;

         foreach (var subViewDescription in SubViewDescriptions)
         {
            subViewDescription.PropertyChanged -= View_PropertyChanged;
         }
      }

      #endregion

      #region Private Methods

      private void SubViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
      {
         if (e.OldItems != null)
         {
            foreach (var oldItem in e.OldItems)
            {
               (oldItem as WorkspaceDescription).PropertyChanged -= View_PropertyChanged;
            }
         }

         if (e.NewItems != null)
         {
            foreach (var newItem in e.NewItems)
            {
               (newItem as WorkspaceDescription).PropertyChanged += View_PropertyChanged;
            }
         }

         OnPropertyChanged(nameof(IsSubViewLoaded));
      }

      private void View_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(IsLoaded)))
         {
            OnPropertyChanged(nameof(IsSubViewLoaded));
         }
      }

      #endregion
   }
}
