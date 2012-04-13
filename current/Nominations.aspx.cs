using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;

public partial class wwwroot_finalsite_Nominations : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = HttpContext.Current.User.Identity.Name.ToString();
        if (!Page.IsPostBack)
        {            
            bindData();            
        }
        
    }

    protected void GridViewNominations_ItemCommand(object sender, GridViewCommandEventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, Page.User.Identity.Name.ToString());

        if (String.Equals(e.CommandName, "accept"))
        {
            if (dbLogic.isUserWTS(userObject.ID, e.CommandArgument.ToString()))
            {
                //change accepted in nomination_accept table to 1
                dbLogic.userAcceptedNom(userObject.ID.ToString(), e.CommandArgument.ToString());

                //confirm labels
                ConfirmLabel.Text = "Thank you for accepting your nomination to the " + e.CommandArgument + " position!";
                ConfirmLabel.Visible = true;
                GridViewNominations.Visible = false;
                PanelNomination.Visible = false;
            }
            else
            {
                HiddenFieldID.Value = dbLogic.selectIDFromPosition(e.CommandArgument.ToString());
                Response.Redirect("~/WTS.aspx/" + HiddenFieldID.Value);
            }
        }
        else if (String.Equals(e.CommandName.ToString(), "reject"))
        {
            dbLogic.userRejectedNom(userObject.ID.ToString(), e.CommandArgument.ToString());
            ConfirmLabel.Text = "Your rejection to the " + e.CommandArgument + " position has been acknowleged.";
            ConfirmLabel.Visible = true;
            bindData();
        }
    }

    protected void bindData()
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, Page.User.Identity.Name.ToString());

        dbLogic.selectAllUserNoms(userObject.ID.ToString());
        DataSet nominationSet = dbLogic.getResults();
        GridViewNominations.DataSource = nominationSet;
        GridViewNominations.DataBind();
    }

}