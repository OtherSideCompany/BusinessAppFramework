using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace OtherSideCore.Wpf
{
   public static class WpfHelper
   {
      public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
      {
         if (depObj != null)
         {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
               DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
               if (child != null && child is T)
               {
                  yield return (T)child;
               }

               foreach (T childOfChild in FindVisualChildren<T>(child))
               {
                  yield return childOfChild;
               }
            }
         }
      }

      public static T FindChildByName<T>(DependencyObject parent, string childName) where T : DependencyObject
      {
         if (parent == null) return null;

         T foundChild = null;

         int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
         for (int i = 0; i < childrenCount; i++)
         {
            var child = VisualTreeHelper.GetChild(parent, i);

            T childType = child as T;
            if (childType == null)
            {
               foundChild = FindChildByName<T>(child, childName);
               if (foundChild != null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
               var frameworkElement = child as FrameworkElement;

               if (frameworkElement != null && frameworkElement.Name == childName)
               {
                  foundChild = (T)child;
                  break;
               }
            }
            else
            {
               foundChild = (T)child;
               break;
            }
         }

         return foundChild;
      }

      public static T FindParent<T>(DependencyObject child, string parentName) where T : DependencyObject
      {
         if (child == null) return null;

         T foundParent = null;
         var currentParent = VisualTreeHelper.GetParent(child);

         do
         {
            var frameworkElement = currentParent as FrameworkElement;
            if (frameworkElement.Name == parentName && frameworkElement is T)
            {
               foundParent = (T)currentParent;
               break;
            }

            currentParent = VisualTreeHelper.GetParent(currentParent);

         } while (currentParent != null);

         return foundParent;
      }

      public static T FindParent<T>(FrameworkElement child) where T : FrameworkElement
      {
         var parent = VisualTreeHelper.GetParent(child);

         if (parent != null && !(parent is T))
         {
            return FindParent<T>((FrameworkElement)parent);
         }

         return (T)parent;
      }
   }
}
