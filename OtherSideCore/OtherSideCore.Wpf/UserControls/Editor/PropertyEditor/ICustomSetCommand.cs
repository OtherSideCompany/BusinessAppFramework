using System.Windows.Input;

namespace OtherSideCore.Wpf.UserControls.Editor.PropertyEditor
{
   internal interface ICustomSetCommand
   {
      ICommand CustomSetCommand { get; set; }
      string CustomSetCommandIconResource { get; set; }
   }
}
