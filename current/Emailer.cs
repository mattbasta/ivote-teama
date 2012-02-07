﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;


/// <summary>
/// Created by: Adam Blank, emailer.cs, 9/23/2011
/// generic emailer class that sends an email through the server.
/// 
/// Last modified: 2/6/2012
/// </summary>
/// 

public class emailer
{
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
        fromAddress = "\"iVote System\" <" + System.Configuration.ConfigurationManager.AppSettings["fromAddress"] + ">";
        host = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];
        userName = System.Configuration.ConfigurationManager.AppSettings["smtpUser"];
        userPass = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"];
        portNumber = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);
        enableSSL = false;
        myCredentials = new System.Net.NetworkCredential(userName, userPass);
    }

    //generic method for sending email
    private string send(MailMessage mail)
    {
        try
        {

            SmtpClient smtp = new SmtpClient();
            smtp.Host = host;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = true;
            //System.Net.NetworkCredential myCredentials = new System.Net.NetworkCredential();
           // myCredentials.UserName = userName;
            //myCredentials.Password = userPass;
            //smtp.Credentials = myCredentials;
            smtp.Port = 25;
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
        try
        {
            MailMessage mail = new MailMessage();
            
            //adds "to" email addresses from array
            for (int i = 0; i < addresses.Length; i++)
            {
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


