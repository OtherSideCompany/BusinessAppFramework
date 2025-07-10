using System.Reflection;
using System;
using System.Windows;
using System.Windows.Controls;
using OtherSideCore.Adapter.DomainObjectInteraction;
using System.Linq;
using OtherSideCore.Adapter.Attributes;
using OtherSideCore.Adapter.Services;

namespace OtherSideCore.Wpf.UserControls.Editor
{
   /// <summary>
   /// Interaction logic for DomainObjectEditorPropertiesEdition.xaml
   /// </summary>
   public partial class DomainObjectEditorPropertiesEdition : UserControl
   {
      #region Constructor

      public DomainObjectEditorPropertiesEdition()
      {
         InitializeComponent();

         DataContextChanged += (_, __) => BuildEditorFromViewModel();
      }

      #endregion

      #region Private Methods

      private void BuildEditorFromViewModel()
      {
         if (DataContext is IDomainObjectEditorViewModel domainObjectEditorViewModel)
         {
            var viewModel = domainObjectEditorViewModel.DomainObjectViewModel;

            var monitoredPropertiesInfos = viewModel.GetType()
                                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(p => Attribute.IsDefined(p, typeof(MonitoredProperty)) &&
                                                                !Attribute.IsDefined(p, typeof(EditorIgnore)))
                                                    .OrderBy(p => p.GetCustomAttribute<DisplayIndex>()?.Index ?? int.MaxValue)
                                                    .ToList();


            EditorGrid.RowDefinitions.Clear();
            EditorGrid.Children.Clear();

            var factory = domainObjectEditorViewModel.DomainObjectEditorViewModelDependencies.PropertyEditorFactory;

            int row = 0;

            foreach (var propInfo in monitoredPropertiesInfos)
            {
               string displayName = GetDisplayName(propInfo, domainObjectEditorViewModel.DomainObjectViewModel.LocalizationService);
               AddLabelToEditorGrid(displayName, row);

               var editorView = (UIElement)factory.CreateEditor(propInfo, viewModel, domainObjectEditorViewModel);
               AddViewToEditorGrid(editorView, row);

               row++;
            }
         }
      }

      private string GetDisplayName(PropertyInfo propertyInfo, ILocalizationService localizationService)
      {
         var editorLabelAttrbute = propertyInfo.GetCustomAttribute<DisplayKey>();

         return localizationService.GetString(editorLabelAttrbute?.Key);
      }

      private void AddLabelToEditorGrid(string propertyLabel, int rowIndex)
      {
         EditorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

         var label = new TextBlock
         {
            Text = propertyLabel,
            Style = TryFindResource("PropertyLabelTextBlock") as Style
         };

         Grid.SetRow(label, rowIndex);
         Grid.SetColumn(label, 0);
         EditorGrid.Children.Add(label);
      }

      private void AddViewToEditorGrid(UIElement editorView, int rowIndex)
      {
         Grid.SetRow(editorView, rowIndex);
         Grid.SetColumn(editorView, 1);
         EditorGrid.Children.Add(editorView);
      }

      #endregion
   }
}
