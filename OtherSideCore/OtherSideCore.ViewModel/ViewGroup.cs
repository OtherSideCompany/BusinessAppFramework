using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace OtherSideCore.ViewModel
{
   public class ViewGroup : ViewBase
   {
      #region Fields

      private bool m_IsExpanded;

      private ObservableCollection<View> m_SubViews;

      #endregion

      #region Properties

      public bool IsExpanded
      {
         get => m_IsExpanded;
         set => SetProperty(ref m_IsExpanded, value);
      }

      public ObservableCollection<View> SubViews
      {
         get => m_SubViews;
         set => SetProperty(ref m_SubViews, value);
      }

      public bool IsSubViewLoaded
      {
         get
         {
            return SubViews.Any(m => m.IsLoaded);
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewGroup(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, string name, object iconResource) : base(serviceProvider, loggerFactory, name, null, iconResource)
      {
         SubViews = new ObservableCollection<View>();

         ViewNavigationPath = Name;

         SubViews.CollectionChanged += SubViews_CollectionChanged;
      }

      #endregion

      #region Public Methods   

      public override void Dispose()
      {
         base.Dispose();

         SubViews.CollectionChanged -= SubViews_CollectionChanged;

         foreach (var module in SubViews)
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
               (oldItem as View).PropertyChanged -= View_PropertyChanged;
            }
         }

         if (e.NewItems != null)
         {
            foreach (var newItem in e.NewItems)
            {
               (newItem as View).PropertyChanged += View_PropertyChanged;
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
