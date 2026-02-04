using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;

namespace OtherSideCore.Application.Tests.Services
{
   public class DefaultDomainObject : DomainObject
   {
      [DefaultValue(GlobalVariables.DefaultString)]
      public string RandomProperty { get; set; }
   }
}
