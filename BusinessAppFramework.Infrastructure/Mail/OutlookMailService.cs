using BusinessAppFramework.Application.Mail;
using System;
using System.IO;
using System.Text;

namespace BusinessAppFramework.Infrastructure.Mail
{
   public class OutlookMailService : IMailService
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public OutlookMailService()
      {

      }

      #endregion

      #region Public Methods

      public void Send(Application.Mail.Mail mail)
      {
         try
         {
            Microsoft.Office.Interop.Outlook.Application outlookApplication = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook._MailItem mailItem = (Microsoft.Office.Interop.Outlook._MailItem)outlookApplication.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

            mailItem.To = mail.To;
            mailItem.Subject = mail.Subject;
            mailItem.HTMLBody = mail.Body.Replace("\n", "<br/>") + ReadSignature();

            foreach (var attachment in mail.Attachments)
            {
               mailItem.Attachments.Add(attachment.FullName, Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
            }

            mailItem.Display(true);
         }
         catch (Exception e)
         {
            var message = "Tentative d'envoi du mail:\n\n";

            message += "A : " + mail.To;
            message += "\nSujet : " + mail.Subject;
            message += "\nMail : " + mail.Body;

            foreach (var attachment in mail.Attachments)
            {
               message += "\nPièce jointe : " + attachment.FullName;
            }

            message += "\n\nProblème lors de la création du mail dans Outlook.\n\nVérifiez que votre instance Outlook est bien démarée, ou tentez de la redémarrer.";

            throw new InvalidOperationException(message, e);
         }
      }

      #endregion

      #region Private Methods

      private static string ReadSignature()
      {
         string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
         string signature = string.Empty;
         DirectoryInfo dirInfo = new DirectoryInfo(appDataDir);

         if (dirInfo.Exists)
         {
            FileInfo[] filesSignature = dirInfo.GetFiles("*.htm");

            if (filesSignature.Length > 0)
            {
               StreamReader sr = new StreamReader(filesSignature[0].FullName, Encoding.Default);
               signature = sr.ReadToEnd();

               if (!string.IsNullOrEmpty(signature))
               {
                  string fileName = filesSignature[0].Name.Replace(filesSignature[0].Extension, string.Empty);
                  signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
               }
            }
         }

         return signature;
      }

      #endregion
   }
}
