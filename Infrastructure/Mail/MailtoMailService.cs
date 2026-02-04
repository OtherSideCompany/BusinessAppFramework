using Application.Mail;
using System;
using System.Diagnostics;
using System.Net;

namespace Infrastructure.Mail
{
   public class MailtoMailService : IMailService
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public MailtoMailService()
      {

      }

      #endregion

      #region Public Methods

      public void Send(Application.Mail.Mail mail)
      {
         string mailtoUri = "";

         try
         {
            string to = WebUtility.UrlEncode(mail.To);
            string subject = Uri.EscapeDataString(mail.Subject);
            string body = Uri.EscapeDataString(mail.Body.Replace("\n", "\r\n"));

            mailtoUri = $"mailto:{to}?subject={subject}&body={body}";

            Process.Start(new ProcessStartInfo(mailtoUri) { UseShellExecute = true });
         }
         catch (Exception e)
         {
            string message = "Tentative d'envoi du mail:\n\n";
            message += $"A : {mail.To}\n";
            message += $"Sujet : {mail.Subject}\n";
            message += $"Mail : {mail.Body}\n";
            foreach (var attachment in mail.Attachments)
            {
               message += $"Pièce jointe : {attachment.FullName}\n";
            }
            message += $"\n\nProblème lors de l'ouverture du mail. Vérifiez que votre client mail est bien configuré.\n\n{mailtoUri}";

            throw new InvalidOperationException(message, e);
         }
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
