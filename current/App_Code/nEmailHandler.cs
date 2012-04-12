using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;
using System.Diagnostics;

/// <summary>
/// Handles email notifications added during Spring 2012 for committee elections
/// </summary>
public class nEmailHandler
{
    emailer sendEmail;
    String emailBaseUrl;
    databaseLogic dbLogic = new databaseLogic();
    string template;
    string footer;

	public nEmailHandler()
	{

        sendEmail = new emailer();
        emailBaseUrl = System.Configuration.ConfigurationManager.AppSettings["baseUrl"];

	}

    public void sendWTS(DatabaseEntities.CommitteeElection committeeElection, List<DatabaseEntities.User> userList)
    {
        string[] allAddresses = new string[userList.Count];

        int i = 0;
        foreach (DatabaseEntities.User user in userList)
        {
            allAddresses[i] = user.Email;
            i++;
        }

        //Retrieve committee information
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.Committee committee = DatabaseEntities.Committee.FindCommittee(session, committeeElection.PertinentCommittee);

        if (committee == null)
            return;

        loadTemplate("emailCommitteeWTS");
        string subject = "APSCUF iVote Willingness to Serve - " + committee.Name;
        string message = template;

        message = message.Replace("##BASEURL##", emailBaseUrl);
        message = message.Replace("##ID##", committeeElection.ID.ToString());

        message += footer;
        sendEmail.sendEmailToList(allAddresses, message, subject);
    }

    public void sendConfirmationEmail(DatabaseEntities.User user, string messageSubject, string templateFile)
    {
        loadTemplate(templateFile);
        string message = template;

        string code_confirm = CreateRandomString(64);
        string code_reject = CreateRandomString(64);

        message = message.Replace("##BASEURL##", emailBaseUrl);
        message = message.Replace("##FIRSTNAME##", user.FirstName);
        message = message.Replace("##LASTNAME##", user.LastName);
        message = message.Replace("##EMAIL##", user.Email);
        message = message.Replace("##CODE##", code_confirm);
        message += footer;

        dbLogic.insertCodes(user.ID, code_confirm, code_reject);

        string[] receiver = { user.Email };

        sendEmail.sendEmailToList(receiver, message, messageSubject);

    }


    private void loadTemplate(string sourceFile)
    {
        string fileName = HttpContext.Current.Server.MapPath("~/App_Data/emailtemplates/" + sourceFile + ".txt");
        template = File.ReadAllText(fileName);

        fileName = HttpContext.Current.Server.MapPath("~/App_Data/emailtemplates/emailFooter.txt");
        footer = File.ReadAllText(fileName);
    }

    private static string CreateRandomString(int length)
    {
        string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        char[] chars = new char[length];
        Random rd = new Random();

        for (int i = 0; i < length; i++)
        {
            chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
        }

        return new string(chars);
    }
}