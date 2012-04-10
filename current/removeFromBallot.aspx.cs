using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;

public partial class wwwroot_finalsite_removeFromBallot : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    VerifyEmail email = new VerifyEmail();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dbLogic.selectInfoForApprovalTable();
            DataSet emailSet = dbLogic.getResults();
            ListViewApproval.DataSource = emailSet;
            ListViewApproval.DataBind();
            loadApprovalInfo();
        }
    }
    
    protected string GetName(int UserID) {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        User u = DatabaseEntities.User.FindUser(session, UserID);
        return u.FirstName + " " + u.LastName;
    }
    
    protected void loadApprovalInfo()
    {
        foreach (ListViewDataItem eachItem in ListViewApproval.Items)
            {
                Label fullname = (Label)eachItem.FindControl("LabelFullname");
                Label position = (Label)eachItem.FindControl("LabelPosition");
                Button remove = (Button)eachItem.FindControl("ButtonRemove");
                HiddenField eligible = (HiddenField)eachItem.FindControl("HiddenFieldEligible");
                if (eligible.Value == "0")
                {
                    fullname.CssClass = "lineThrough";
                    position.CssClass = "lineThrough";
                    remove.Text = "Place Back on Ballot";
                    remove.CommandName = "add";
                }
                else
                {
                    fullname.CssClass = "";
                    position.CssClass = "";
                    remove.Text = "Remove From Ballot";
                    remove.CommandName = "remove";
                }
            }
    }
    
    protected void showFullStatement(object sender, ListViewCommandEventArgs e)
    {
        string[] emailList;
        emailer emailSender = new emailer();

        //grab full list of emails
        emailList = dbLogic.getEmails();

        string[] data = e.CommandArgument.ToString().Split(new char[] { '%' });

        if (String.Equals(e.CommandName, "remove"))
        {
            dbLogic.updateEligible(data[0], "0");
            LabelFeedback.Text = "User successfully removed from the current ballot. Email sent to ALL users alerting them of this action.";
            emailSender.sendEmailToList(emailList, "Hello and greetings from the APSCUF-KU iVote System! <br /><br />This email is to inform you that " + dbLogic.selectFullName(data[1]) + " has been offically removed from the current election ballot. <br /><br />This action performed by the current system administrator and was likely the cause of this person expressing that they no longer wishes to be considered for their nominationed position.<br /><br />Any questions or comments please email the current APSCUF-KU iVote system administrator.<br /><br />Team A<br />CSC 354<br /><br />(PLEASE DO NOT REPLY TO THIS EMAIL.)", dbLogic.selectFullName(data[1]) + " Offically Removed From Election Ballot");
        }

        else if (String.Equals(e.CommandName, "add"))
        {
            dbLogic.updateEligible(data[0], "1");
            LabelFeedback.Text = "User successfully placed back on the current ballot. Email sent to ALL users alerting them of this action.";
            emailSender.sendEmailToList(emailList, "Hello and greetings from the APSCUF-KU iVote System! <br /><br />This email is to inform you that " + dbLogic.selectFullName(data[1]) + " has been offically re-added to the ballot for this election by the current system administrator.<br /><br />Any questions or comments please email the current APSCUF-KU iVote system administrator.<br /><br />Team A<br />CSC 354<br /><br />(PLEASE DO NOT REPLY TO THIS EMAIL.)", dbLogic.selectFullName(data[1]) + " Offically Re-Added To Election Ballot");
        }
        dbLogic.selectInfoForApprovalTable();
        DataSet emailSet = dbLogic.getResults();
        ListViewApproval.DataSource = emailSet;
        ListViewApproval.DataBind();
        loadApprovalInfo();
        
    }

}