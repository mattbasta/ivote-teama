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

public partial class wwwroot_finalsite_RemoveFromBallot : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
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
                    remove.CssClass = "btn btn-success btn-mini";
                    remove.CommandName = "add";
                }
                else
                {
                    fullname.CssClass = "";
                    position.CssClass = "";
                    remove.Text = "Remove From Ballot";
                    remove.CssClass = "btn btn-danger btn-mini";
                    remove.CommandName = "remove";
                }
            }
    }
    
    protected void showFullStatement(object sender, ListViewCommandEventArgs e)
    {
        string[] emailList;
        nEmailHandler emailer = new nEmailHandler();

        //grab full list of emails
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        List<DatabaseEntities.User> userList = DatabaseEntities.User.GetAllUsers(session);


        string[] data = e.CommandArgument.ToString().Split(new char[] { '%' });

        if (String.Equals(e.CommandName, "remove"))
        {
            dbLogic.updateEligible(data[0], "0");
            LabelFeedback.Text = "User successfully removed from the current ballot. Email sent to ALL users alerting them of this action.";
            emailer.sendRemoveFromBallot(userList, GetName(Convert.ToInt32(data[1])));
        }

        else if (String.Equals(e.CommandName, "add"))
        {
            dbLogic.updateEligible(data[0], "1");
            LabelFeedback.Text = "User successfully placed back on the current ballot. Email sent to ALL users alerting them of this action.";
            emailer.sendReAddToBallot(userList, GetName(Convert.ToInt32(data[1])));
        }
        dbLogic.selectInfoForApprovalTable();
        DataSet emailSet = dbLogic.getResults();
        ListViewApproval.DataSource = emailSet;
        ListViewApproval.DataBind();
        loadApprovalInfo();
        
    }

}