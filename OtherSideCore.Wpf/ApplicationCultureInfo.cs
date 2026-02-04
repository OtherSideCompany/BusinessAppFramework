using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;

namespace OtherSideCore.Utils
{
   public class ApplicationCultureInfo
   {
      public static void OverrideCurrentCultureInfo(string cultureTag)
      {
         var vCulture = new CultureInfo(cultureTag);

         Thread.CurrentThread.CurrentCulture = vCulture;
         Thread.CurrentThread.CurrentUICulture = vCulture;
         CultureInfo.DefaultThreadCurrentCulture = vCulture;
         CultureInfo.DefaultThreadCurrentUICulture = vCulture;

         FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
         FrameworkElement.LanguageProperty.OverrideMetadata(typeof(Run), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
      }
   }
}
