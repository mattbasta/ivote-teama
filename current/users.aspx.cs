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

public partial class wwwroot_phase1aSite_users : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            ShowAllUsers();
    }
    
    private void ShowAllUsers() {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        GridViewUsers.DataSource = session.CreateCriteria(typeof(DatabaseEntities.User))
                .List<DatabaseEntities.User>();
        GridViewUsers.DataBind();
    }

    protected void GridViewUsers_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "id"))
            Response.Redirect("UserInfo.aspx?id=" + e.CommandArgument.ToString());
    }

    protected void sorting(object sender, GridViewSortEventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        GridViewUsers.DataSource = session.CreateCriteria(typeof(DatabaseEntities.User))
                                       .AddOrder(new Order(e.SortExpression.ToString(), true))
                                       .List<DatabaseEntities.User>();
        GridViewUsers.DataBind();
    }

    protected void allUsers(object sender, EventArgs e)
    {
        ShowAllUsers();

        btnViewAll.Visible = false;
        txtSearch.Text = "";
    }

    protected void search_adam(object sender, EventArgs e)
    {
        string query = txtSearch.Text;
    
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        GridViewUsers.DataSource = session.CreateCriteria(typeof(DatabaseEntities.User))
                .Add(Restrictions.Or(Restrictions.Like("FirstName", "%" + query + "%"),
                                     Restrictions.Like("LastName", "%" + query + "%")))
                .List<DatabaseEntities.User>();
        GridViewUsers.DataBind();

        if(txtSearch.Text != "")
            btnViewAll.Visible = true;

    }

}