using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendMail : MonoBehaviour
{
    // mailFrom must be Gmail or you have to change smtpServer properties to your mail provider
    // mailFrom must have enabled "Allow less secure applications" in google account and I recommend use for it new mail account for safety
    // https://myaccount.google.com/lesssecureapps
    // mailTo can be from any mail provider

    [Tooltip("Mail from which messages will be sent")]
    public string mailFrom = "[Your mail from which messages will be sent]";
    [Tooltip("Password to mailFrom")]
    public string mailFromPassword = "[Password to mailFrom]";
    [Tooltip("Mail to which you want to send feedback")]
    public string mailTo="[Your feedback mail]";

    public void SendMailMessage(string userMessage)
    {
        // analytics action type
        // mail message
        string message = "Hi\n" +
                         userMessage + "\n" +
                         "----------------------------------------------------------\n";
        // create mail content
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(mailFrom);
        mail.To.Add(mailTo);
        mail.Subject = "ALERT! Your accountability partner needs you!";
        mail.Body = message;
        // gmail server configuration, credentials, certificate etc
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, mailFromPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);

        Debug.Log("Mail send:\n" +message);
    }
}
