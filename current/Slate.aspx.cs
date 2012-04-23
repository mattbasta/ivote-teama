using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;

using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;

//Created by Adam Blank, 11/8/2011

public partial class slate : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, HttpContext.Current.User.Identity.Name);

            dbLogic.selectAllBallotPositions();
            DataSet emailSet = dbLogic.getResults();
            ListViewPositions.DataSource = emailSet;
            ListViewPositions.DataBind();
        }
    }
    
    protected string GetName(int UserID) {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        User u = DatabaseEntities.User.FindUser(session, UserID);
        return u.FirstName + " " + u.LastName;
    }

    protected void ListViewPositions_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "position"))
        {
            dbLogic.selectAllForBallot(e.CommandArgument.ToString());
            DataSet emailSet = dbLogic.getResults();
            ListViewPeople.DataSource = emailSet;
            ListViewPeople.DataBind();
            PanelPeople.Visible = true;
            PanelSelect.Visible = false;

        }
    }

    protected void ListViewPeople_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "id"))
        {
            dbLogic.selectDetailFromWTS(e.CommandArgument.ToString());
            DataSet infoSet = dbLogic.getResults();
            DataRow dr = infoSet.Tables["query"].Rows[0];

            string name = GetName((int)dr["idunion_members"]);
            LabelStatement.Text = "\"" + dr["statement"].ToString() + "\"";

            if (LabelStatement.Text == "\"\"")
                LabelStatement.Text = name + " did not include a personal statement.";

            PanelSelect.Visible = true;
        }
    }
}
