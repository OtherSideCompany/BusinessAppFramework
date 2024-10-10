using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application
{
   public class BatchObservableCollection<T> : ObservableCollection<T>
   {
      private bool _suppressNotification = false;

      public void AddRange(IEnumerable<T> items)
      {
         if (items == null) throw new ArgumentNullException(nameof(items));

         _suppressNotification = true;

         foreach (var item in items)
         {
            Add(item);
         }

         _suppressNotification = false;

         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }

      public void ReplaceItems(IEnumerable<T> items)
      {
         if (items == null) throw new ArgumentNullException(nameof(items));

         _suppressNotification = true;

         Clear();

         foreach (var item in items)
         {
            Add(item);
         }

         _suppressNotification = false;

         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }

      protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
      {
         if (!_suppressNotification)
         {
            base.OnCollectionChanged(e);
         }
      }
   }
}
