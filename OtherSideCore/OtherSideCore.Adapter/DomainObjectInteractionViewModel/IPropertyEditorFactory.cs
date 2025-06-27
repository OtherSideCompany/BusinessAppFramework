using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public interface IPropertyEditorFactory
   {
      object? CreateEditor(PropertyInfo propInfo, DomainObjectViewModel viewModel, IDomainObjectEditorViewModel domainObjectEditorViewModel);
   }
}
