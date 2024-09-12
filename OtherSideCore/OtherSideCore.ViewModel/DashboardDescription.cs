using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace OtherSideCore.ViewModel
{
   public class DashboardDescription : ViewDescriptionBase
   {
      #region Fields

      private bool m_IsExpanded;

      private ObservableCollection<ViewDescription> _subViewDescriptions;

      private DashboardViewModelBase _dashboardViewModelBase;

      #endregion

      #region Properties

      public bool IsExpanded
      {
         get => m_IsExpanded;
         set => SetProperty(ref m_IsExpanded, value);
      }

      public ObservableCollection<ViewDescription> SubViewDescriptions
      {
         get => _subViewDescriptions;
         set => SetProperty(ref _subViewDescriptions, value);
      }

      public DashboardViewModelBase DashboardViewModelBase
      {
         get => _dashboardViewModelBase;
         set => SetProperty(ref _dashboardViewModelBase, value);
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

      public DashboardDescription(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, string name, Type viewModelType, object iconResource) : base(serviceProvider, loggerFactory, name, viewModelType, iconResource)
      {
         SubViewDescriptions = new ObservableCollection<ViewDescription>();

         ViewNavigationPath = Name;

         SubViewDescriptions.CollectionChanged += SubViews_CollectionChanged;
      }

      #endregion

      #region Public Methods  

      public override void InstanciateViewModel()
      {
         DashboardViewModelBase = (DashboardViewModelBase)_serviceProvider.GetService(_viewModelType);

         DashboardViewModelBase.DashboardDescription = this;

         _logger.LogInformation("Displaying view {ViewName} with view model {ViewModelType}", Name, _viewModelType.Name);
      }

      public override void Unload()
      {
         base.Unload();

         DashboardViewModelBase?.Dispose();
         DashboardViewModelBase = null;
      }

      public override void Dispose()
      {
         base.Dispose();

         SubViewDescriptions.CollectionChanged -= SubViews_CollectionChanged;

         foreach (var module in SubViewDescriptions)
         {
            module.PropertyChanged -= View_PropertyChanged;
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
               (oldItem as ViewDescription).PropertyChanged -= View_PropertyChanged;
            }
         }

         if (e.NewItems != null)
         {
            foreach (var newItem in e.NewItems)
            {
               (newItem as ViewDescription).PropertyChanged += View_PropertyChanged;
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
