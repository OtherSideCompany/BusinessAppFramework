using Microsoft.VisualBasic;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Attributes;
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Wpf.UserControls.Editor.PropertyEditor;
using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using SystemAdministration.Desktop.PropertyEditors;

namespace OtherSideCore.Wpf.Factories
{
   public class PropertyEditorFactory : TypeBasedFactory, IPropertyEditorFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public PropertyEditorFactory()
      {
         RegisterEditor<string, StringEditor>(StringEditor.StringEditor_TextProperty, UpdateSourceTrigger.PropertyChanged);
         RegisterEditor<int?, NullableIntegerEditor>(NullableIntegerEditor.NullableIntegerEditor_ValueProperty, UpdateSourceTrigger.LostFocus);
         RegisterEditor<int, IntegerEditor>(IntegerEditor.IntegerEditor_ValueProperty, UpdateSourceTrigger.LostFocus);
         RegisterEditor<bool, BooleanEditor>(BooleanEditor.BooleanEditor_IsCheckedProperty, UpdateSourceTrigger.PropertyChanged);
         RegisterEditor<DateTime?, NullableDateTimeEditor>(NullableDateTimeEditor.NullableDateTimeEditor_SelectedDateProperty, UpdateSourceTrigger.PropertyChanged);
      }

      public object CreateEditor(PropertyInfo propInfo, DomainObjectViewModel viewModel, IDomainObjectEditorViewModel domainObjectEditorViewModel)
      {
         return CreateFromType(propInfo.PropertyType, propInfo, viewModel, domainObjectEditorViewModel);
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods

      protected void RegisterEditor<TProperty, TEditor>(DependencyProperty targetProperty, UpdateSourceTrigger trigger) where TEditor : FrameworkElement, new()
      {
         Register(typeof(TProperty), args =>
         {
            var propInfo = (PropertyInfo)args[0];
            var viewModel = (DomainObjectViewModel)args[1];
            var editorViewModel = (IDomainObjectEditorViewModel)args[2];

            var editor = new TEditor();

            var binding = new Binding(propInfo.Name)
            {
               Source = viewModel,
               UpdateSourceTrigger = trigger,
               Mode = BindingMode.TwoWay
            };

            editor.SetBinding(targetProperty, binding);

            var matchingCustomSetterCommandProperty = editorViewModel.GetType()
                                                                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                                     .FirstOrDefault(p => typeof(ICommand).IsAssignableFrom(p.PropertyType) &&
                                                                                          p.GetCustomAttribute<CustomSetterForProperty>()?.TargetPropertyName == propInfo.Name);

            var iconResourceAttribute = matchingCustomSetterCommandProperty?.GetCustomAttribute<CustomSetterIconResource>();

            if (matchingCustomSetterCommandProperty != null && editor is ICustomSetCommand customSetCommand)
            {
               var command = (ICommand)matchingCustomSetterCommandProperty.GetValue(editorViewModel);
               customSetCommand.CustomSetCommand = command;
               customSetCommand.CustomSetCommandIconResource = iconResourceAttribute.IconResource;
            }

            return editor;
         });
      }

      protected IDomainObjectSelectorViewModel? FindSelectorForProperty(string propertyName, IDomainObjectEditorViewModel editorViewModel)
      {
         var selector = editorViewModel.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => typeof(IDomainObjectSelectorViewModel).IsAssignableFrom(p.PropertyType))
            .FirstOrDefault(p =>
            {
               var targetAttr = p.GetCustomAttribute<SelectorTargetViewModelPropertyAttribute>();
               return targetAttr != null && targetAttr.TargetPropertyName == propertyName;
            });

         return selector?.GetValue(editorViewModel) as IDomainObjectSelectorViewModel;
      }

      protected void RegisterSelectorPropertyEditor<T>() where T : class
      {
         Register(typeof(T), args =>
         {
            var propInfo = (PropertyInfo)args[0];
            var viewModel = (DomainObjectViewModel)args[1];
            var editorViewModel = (IDomainObjectEditorViewModel)args[2];

            var editor = new SelectorPropertyEditor();

            editor.SetBinding(SelectorPropertyEditor.SelectorPropertyEditor_ValueProperty, new Binding(propInfo.Name)
            {
               Source = viewModel,
               Mode = BindingMode.OneWay,
               UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });           

            var selector = FindSelectorForProperty(propInfo.Name, editorViewModel);

            if (selector != null)
            {
               editor.UserPropertyEditor_DisplaySelectorCommand = selector.DisplaySelectorAsyncCommand;
            }
            else
            {
               throw new System.Exception($"No selector found for property '{propInfo.Name}' in editor view model '{editorViewModel.GetType().Name}'.");
            }

            return editor;
         });
      }

      #endregion
   }
}
