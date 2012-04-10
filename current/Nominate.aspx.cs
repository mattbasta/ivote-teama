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
using NHibernate.Criterion;
using NHibernate.Cfg;

public partial class wwwroot_phase1aSite_nominate : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    emailer emailHelper = new emailer();
    protected void Page_PreRender(object sender, EventArgs e)
    {

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        string fullPath = Request.PathInfo;
        string username = Page.User.Identity.Name.ToString();
        if (!Page.IsPostBack)
        {
            string queryVal = Request.QueryString["position"]; //variable name of querystring
            string positionTitle = dbLogic.selectPositionFromID(queryVal);
            //Bounce invalid or missing position id
            if (positionTitle == "")
                Response.Redirect("home.aspx");
            LabelHeader.Text = positionTitle + " Nomination";
            HiddenFieldPosition.Value = positionTitle;
        }
    }


    protected void search(object sender, EventArgs e)
    {
        string query = txtSearch.Text;

        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ListViewUsers.DataSource = session.CreateCriteria(typeof(DatabaseEntities.User))
                .Add(Restrictions.Or(Restrictions.Like("FirstName", "%" + query + "%"),
                                     Restrictions.Like("LastName", "%" + query + "%")))
                .List<DatabaseEntities.User>();

        //.SelectPeopleFromSearch(txtSearch.Text);
        //DataSet emailSet = dbLogic.getResults();
        //ListViewUsers.DataSource = emailSet;
        ListViewUsers.DataBind();
        ListViewUsers.Visible = true;

        foreach (ListViewDataItem myItem in ListViewUsers.Items)
        {
            DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, HttpContext.Current.User.Identity.Name);

            Button Nominate = (Button)myItem.FindControl("ButtonNominate");
            //If the current user's name appears in the results list, the nominate button is disabled
            if(Nominate.CommandArgument.ToString() == userObject.ID.ToString())
            {
                Nominate.ToolTip = "To nominate yourself, click \"Nominate Yourself\" on the homepage";
                Nominate.Enabled = false;
            }
        }
        //loadConfirmPopup();

        if (txtSearch.Text != "")
            btnViewAll.Visible = true;
    }

    protected void ListViewUsers_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "nominate"))
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            DatabaseEntities.User userObjectNominee = DatabaseEntities.User.FindUser(session, int.Parse(e.CommandArgument.ToString()));

            LabelComplete.Text = "Thank you for nominating " + userObjectNominee.FirstName + " " + userObjectNominee.LastName + " for " + HiddenFieldPosition.Value;

            PanelSearch.Visible = false;
            PanelComplete.Visible = true;
            PanelComplete.Enabled = true;

            DatabaseEntities.User userObjectSubmitter = DatabaseEntities.User.FindUser(session, HttpContext.Current.User.Identity.Name);

            string[] info = { e.CommandArgument.ToString(), userObjectSubmitter.ID.ToString(), HiddenFieldPosition.Value };

            //inserts data into the db
            dbLogic.insertNominationAccept(info);

            string[] emailAddress = {userObjectNominee.Email};

            //send an email to the user nominated
            emailHelper.sendEmailToList(emailAddress, userObjectSubmitter.FirstName + " " + userObjectSubmitter.LastName + " has nominated you for the position of " + HiddenFieldPosition.Value + " for the next voting period!<br /><br />To be fully nominated you must first accept this nomination,<br /> then fill out the digital \"Willingness To Serve\" form.<br />To accept (or reject) this nomination, log into the <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + "/\" target=\"_blank\"> Kutztown iVote website</a>.<br /><br />The iVote System Team", "You've been nominated for " + HiddenFieldPosition.Value);
        }
    }

    protected void clear(object sender, EventArgs e)
    {
        ListViewUsers.Visible = false;
        btnViewAll.Visible = false;
    }

}
