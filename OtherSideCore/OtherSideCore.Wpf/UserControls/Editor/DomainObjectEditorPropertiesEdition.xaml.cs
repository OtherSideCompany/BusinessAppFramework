using System.Reflection;
using System;
using System.Windows;
using System.Windows.Controls;
using OtherSideCore.Adapter.DomainObjectInteraction;
using System.Linq;
using System.Windows.Data;
using OtherSideCore.Adapter.Attributes;
using OtherSideCore.Wpf.UserControls.Editor.PropertyEditor;

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
                                                    .OrderBy(p => p.GetCustomAttribute<EditorIndex>()?.Index ?? int.MaxValue)
                                                    .ToList();


            EditorGrid.RowDefinitions.Clear();
            EditorGrid.Children.Clear();

            var factory = domainObjectEditorViewModel.DomainObjectEditorViewModelDependencies.PropertyEditorFactory;

            int row = 0;

            foreach (var propInfo in monitoredPropertiesInfos)
            {
               EditorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

               string displayName = GetDisplayName(propInfo);

               var label = new TextBlock
               {
                  Text = displayName,
                  Style = TryFindResource("PropertyLabelTextBlock") as Style
               };

               Grid.SetRow(label, row);
               Grid.SetColumn(label, 0);
               EditorGrid.Children.Add(label);

               var editorView = (UIElement)factory.CreateEditor(propInfo, viewModel, domainObjectEditorViewModel);

               Grid.SetRow(editorView, row);
               Grid.SetColumn(editorView, 1);
               EditorGrid.Children.Add(editorView);

               row++;
            }
         }
      }

      private string GetDisplayName(PropertyInfo propertyInfo)
      {
         var editorLabelAttrbute = propertyInfo.GetCustomAttribute<EditorLabel>();

         return !string.IsNullOrWhiteSpace(editorLabelAttrbute?.Label) ? editorLabelAttrbute.Label : propertyInfo.Name;
      }

      #endregion
   }
}
