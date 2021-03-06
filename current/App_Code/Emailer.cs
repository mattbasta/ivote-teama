using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;


/// <summary>
/// Created by: Adam Blank, emailer.cs, 9/23/2011
/// generic emailer class that sends an email through the server.
/// 
/// Last modified: 2/23/2012
/// </summary>
/// 

public class emailer
{
    private bool enabled;

    private string host;
    private string fromAddress;
    private string userName;
    private string userPass;
    private System.Net.NetworkCredential myCredentials;
    private bool enableSSL;
    private int portNumber;
    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

    public emailer()
    {
        enabled = System.Configuration.ConfigurationManager.AppSettings["smtpEnabled"] == "true";

        if (!enabled)
            return;

        fromAddress = "\"iVote System\" <" + System.Configuration.ConfigurationManager.AppSettings["fromAddress"] + ">";
        host = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];
        userName = System.Configuration.ConfigurationManager.AppSettings["smtpUser"];
        userPass = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"];
        portNumber = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);
        enableSSL = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpEnableSSL"]);
        myCredentials = new System.Net.NetworkCredential(userName, userPass);
    }

    //generic method for sending email
    private string send(MailMessage mail)
    {
        if (!enabled) return "Emails are not enabled.";

        try
        {

            SmtpClient smtp = new SmtpClient();
            smtp.Host = host;
            smtp.EnableSsl = enableSSL;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = myCredentials;
            smtp.Port = portNumber;
            smtp.Send(mail);
            return "Email Sent Successfully";
        }
        catch (Exception e)
        {
            return "An error was encountered when sending this email.  The error generated by the server was: " + e.Message + ".";
        }
    }

    //sends email to a list of addresses inside of an array (array can hold just 1 address as well)
    public string sendEmailToList(string[] addresses, string message, string subject)
    {
        if (!enabled) return "Emails are not enabled.";
        try
        {
            MailMessage mail = new MailMessage();

            //adds "to" email addresses from array
            for (int i = 0; i < addresses.Length; i++)
            {
                if (!addresses[i].Contains("@"))
                    continue;

                mail.Bcc.Add(addresses[i]);
            }
            mail.From = new MailAddress(fromAddress);
            mail.Subject = subject;
            string Body = message;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            return send(mail);
        }
        catch (Exception e)
        {
            return "An error occured when generating this email.  The error generated by the server was: " + e.Message + ".";
        }
    }


}


