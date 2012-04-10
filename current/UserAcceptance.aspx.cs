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

public partial class finalsite_UserAcceptance : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    VerifyEmail sendEmail = new VerifyEmail();
    string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = HttpContext.Current.User.Identity.Name.ToString();
        if (!Page.IsPostBack)
        {
            bindData();
        }
    }

    protected void GridViewNominations_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        
        if (String.Equals(e.CommandName, "accept"))
        {            
            HiddenFieldID.Value = dbLogic.selectIDFromPosition(e.CommandArgument.ToString());
            Response.Redirect("~/WTS.aspx?position=" + HiddenFieldID.Value);
        }
        else if (String.Equals(e.CommandName, "reject"))
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, username);

            dbLogic.userRejectedNom(userObject.ID.ToString(), e.CommandArgument.ToString());
            
            //rejection labels
            ConfirmLabel.Text = "Thank you, your rejection  to the " + e.CommandArgument + " position has been noted.";
            ConfirmLabel.Visible = true;
            //GridViewNominations.Visible = false;
        }
    }

    protected void bindData()
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, username);

        dbLogic.selectAllUserNoms(userObject.ID.ToString());
        DataSet nominationSet = dbLogic.getResults();
        GridViewNominations.DataSource = nominationSet;
        GridViewNominations.DataBind();
    }
}