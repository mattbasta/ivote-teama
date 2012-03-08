using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;

/// <summary>
/// Handles email notifications add during Spring 2012 for committee elections
/// </summary>
public class nEmailHandler
{
    emailer sendEmail;
    String emailBaseUrl;

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

        string subject = "APSCUF iVote Willingness to Serve - " + committee.Name;
        string message = "http://" + emailBaseUrl + "/wts.aspx?election=" + committeeElection.ID;
        sendEmail.sendEmailToList(allAddresses, message, subject);
    }
}