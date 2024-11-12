using OtherSideCore.Adapter;
using OtherSideCore.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for FileDropView.xaml
   /// </summary>
   public partial class FileDropView : UserControl
   {
      public static readonly DependencyProperty FileDropUserControl_IconColorProperty =
          DependencyProperty.Register("FileDropUserControl_IconColor", typeof(SolidColorBrush), typeof(FileDropView), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush FileDropUserControl_IconColor
      {
         get { return (SolidColorBrush)GetValue(FileDropUserControl_IconColorProperty); }
         set { SetValue(FileDropUserControl_IconColorProperty, value); }
      }

      public static readonly DependencyProperty FileDropUserControl_OverBorderColorProperty =
          DependencyProperty.Register("FileDropUserControl_OverBorderColor", typeof(SolidColorBrush), typeof(FileDropView), new UIPropertyMetadata(Brushes.Gray));

      public SolidColorBrush FileDropUserControl_OverBorderColor
      {
         get { return (SolidColorBrush)GetValue(FileDropUserControl_OverBorderColorProperty); }
         set { SetValue(FileDropUserControl_OverBorderColorProperty, value); }
      }

      private static readonly DependencyPropertyKey IsFileOveredPropertyKey =
         DependencyProperty.RegisterReadOnly("IsFileOvered", typeof(bool), typeof(FileDropView), new PropertyMetadata(false));

      public static readonly DependencyProperty IsFileOveredProperty = IsFileOveredPropertyKey.DependencyProperty;

      public bool IsFileOvered
      {
         get { return (bool)GetValue(IsFileOveredProperty); }
         private set { SetValue(IsFileOveredPropertyKey, value); }
      }

      public FileDropView()
      {
         InitializeComponent();
      }      

      private void DropFileGrid_DragEnter(object sender, DragEventArgs e)
      { 
         if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent("FileGroupDescriptor"))
         {
            IsFileOvered = true;
         }
      }

      private void DropFileGrid_DragLeave(object sender, DragEventArgs e)
      {
         IsFileOvered = false;
      }

      private void DropFileGrid_Drop(object sender, DragEventArgs e)
      {
         var viewModel = (this.DataContext as IFileDropZone);

         if (e.Data.GetDataPresent(DataFormats.FileDrop))
         {
            try
            {
               var @object = e.Data.GetData(DataFormats.FileDrop);
               string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
               viewModel.DropFiles(files.Select(f => new ManagedFile(new FileInfo(f))).ToList());
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
               System.Diagnostics.Debug.WriteLine($"Error accessing FileDrop data: {ex.Message}");
            }           
         }
         else if (e.Data.GetDataPresent("FileGroupDescriptor"))
         {
            var filenameBuilder = new StringBuilder();
            var ms = (MemoryStream)e.Data.GetData("FileGroupDescriptor");
            ms.Position = 76;
            char a;
            while ((a = (char)ms.ReadByte()) != 0)
            {
               filenameBuilder.Append(a);
            }

            var rawContent = e.Data.GetData("FileContents") as MemoryStream;

            viewModel.DropFiles(new List<ManagedFile>() { new ManagedFile(filenameBuilder.ToString(), rawContent)});
         }

         IsFileOvered = false;
      }
   }
}
