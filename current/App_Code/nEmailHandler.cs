﻿using System;
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

/// <summary>
/// Handles email notifications added during Spring 2012 for officer and committee elections
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

    /// <summary>
    /// Method to handle all email sending for phase changes in committee elections.
    /// </summary>
    /// <param name="committeeElection">Pertinent committee election</param>
    /// <param name="userList">List of users to receieve email</param>
    /// <param name="templateName">Template file to reference</param>
    public void sendGenericCommitteePhase(DatabaseEntities.CommitteeElection committeeElection, List<DatabaseEntities.User> userList, string templateName)
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

        loadTemplate(templateName);
        string subject = "APSCUF Committee Election - " + committee.Name;
        string message = template;

        message = message.Replace("##BASEURL##", emailBaseUrl);
        message = message.Replace("##ID##", committeeElection.ID.ToString());

        message += footer;
        sendEmail.sendEmailToList(allAddresses, message, subject);
    }

    /// <summary>
    /// Send user creation or reset confirmation email
    /// </summary>
    /// <param name="user">New users being confirmed</param>
    /// <param name="messageSubject">Email subject (new or password reset)</param>
    /// <param name="templateFile">Template file to reference</param>
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

    /// <summary>
    /// Sends email when user is removed from ballot.
    /// </summary>
    /// <param name="userList">User list to send email to</param>
    /// <param name="fullName">Full name of user removed</param>
    public void sendRemoveFromBallot(List<DatabaseEntities.User> userList, string fullName)
    {
        string[] allAddresses = new string[userList.Count];

        int i = 0;
        foreach (DatabaseEntities.User user in userList)
        {
            allAddresses[i] = user.Email;
            i++;
        }

        loadTemplate("officerRemoveFromBallot");
        string message = template;

        message = message.Replace("##FULLNAME##", fullName);

        message += footer;

        sendEmail.sendEmailToList(allAddresses, message, fullName + " offically removed from election ballot");

    }

    /// <summary>
    /// Sends email when user is re-added to ballot.
    /// </summary>
    /// <param name="userList">User list to send email to</param>
    /// <param name="fullName">Full name of user added</param>
    public void sendReAddToBallot(List<DatabaseEntities.User> userList, string fullName)
    {
        string[] allAddresses = new string[userList.Count];

        int i = 0;
        foreach (DatabaseEntities.User user in userList)
        {
            allAddresses[i] = user.Email;
            i++;
        }

        loadTemplate("officerReaddedToBallot");
        string message = template;

        message = message.Replace("##FULLNAME##", fullName);

        message += footer;

        sendEmail.sendEmailToList(allAddresses, message, fullName + " offically readded to election ballot");

    }

    /// <summary>
    /// Sends phase change emails for all officer elections
    /// </summary>
    /// <param name="templateFile">Template file to reference</param>
    /// <param name="subject">Emaul subject</param>
    public void sendGenericOfficerPhase(string templateFile, string subject)
    {

        loadTemplate(templateFile);
        string message = template;

        message = message.Replace("##BASEURL##", emailBaseUrl);

        message += footer;

        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        if (templateFile == "officerPhaseSlate")
        {
            sendEmail.sendEmailToList(dbLogic.getNECEmails(session), message, subject);
        }
        else if (templateFile == "officerPhaseApproval")
        {
            sendEmail.sendEmailToList(dbLogic.getAdminEmails(session), message, subject);
        }
        else if (templateFile == "officerPhaseAccept1")
        {
            sendEmail.sendEmailToList(dbLogic.getNullEmails(), message, subject);
        }
        else
        {
            sendEmail.sendEmailToList(getAllEmails(session), message, subject);
        }

    }

    /// <summary>
    /// Sends email to notify user they have been nominated
    /// </summary>
    /// <param name="user">User to be notified</param>
    public void sendApproveEligibility(DatabaseEntities.User user)
    {

        loadTemplate("officerApproveEligibility");
        string message = template;

        message += footer;

        string[] receiver = { user.Email };

        sendEmail.sendEmailToList(receiver, message, "APSCUF iVote Nomination Approved");

    }

    /// <summary>
    /// Sends email to notify user their eligibility has been revoked
    /// </summary>
    /// <param name="user">User to be notified</param>
    public void sendDenyEligibility(DatabaseEntities.User user)
    {

        loadTemplate("officerApproveEligibility");
        string message = template;

        message += footer;

        string[] receiver = { user.Email };

        sendEmail.sendEmailToList(receiver, message, "APSCUF iVote Nomination Eligibility Notice");

    }

    /// <summary>
    /// Loads template file for email
    /// </summary>
    /// <param name="sourceFile">File name to load (no .txt)</param>
    private void loadTemplate(string sourceFile)
    {
        string fileName = HttpContext.Current.Server.MapPath("~/App_Data/emailtemplates/" + sourceFile + ".txt");
        template = File.ReadAllText(fileName);

        fileName = HttpContext.Current.Server.MapPath("~/App_Data/emailtemplates/emailFooter.txt");
        footer = File.ReadAllText(fileName);
    }

    /// <summary>
    /// Generates a random string to store for password confirmation
    /// </summary>
    /// <param name="length">Length of string to generate</param>
    /// <returns>Random string</returns>
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

    /// <summary>
    /// Gets an array of all email address in the database
    /// </summary>
    /// <param name="session">NHibernate session</param>
    /// <returns>Array of email addresses</returns>
    private string[] getAllEmails(ISession session)
    {
        List<DatabaseEntities.User> userList = DatabaseEntities.User.GetAllUsers(session);

        string[] allAddresses = new string[userList.Count];

        int i = 0;
        foreach (DatabaseEntities.User user in userList)
        {
            allAddresses[i] = user.Email;
            i++;
        }

        return allAddresses;
    }
}