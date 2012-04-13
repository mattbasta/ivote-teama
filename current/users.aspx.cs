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
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            update_results();
    }

    protected void GridViewUsers_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "id"))
            Response.Redirect("UserInfo.aspx?id=" + e.CommandArgument.ToString());
    }

    protected void sorting(object sender, GridViewSortEventArgs e)
    {
        Sort.Value = e.SortExpression.ToString();
        update_results();
    }

    protected void allUsers(object sender, EventArgs e)
    {
        Query.Value = "";
        update_results();

        btnViewAll.Visible = false;
        txtSearch.Text = "";
    }

    protected void search_adam(object sender, EventArgs e)
    {
        Query.Value = txtSearch.Text;
        update_results();

        if(txtSearch.Text != "")
            btnViewAll.Visible = true;

    }
    
    private void update_results() {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ICriteria criteria = session.CreateCriteria(typeof(DatabaseEntities.User));
        if(Query.Value != "")
            criteria = criteria.Add(Restrictions.Or(Restrictions.Like("FirstName", "%" + Query.Value + "%"),
                                                    Restrictions.Like("LastName", "%" + Query.Value + "%")));
        if(Sort.Value != "")
            criteria = criteria.AddOrder(new Order(Sort.Value, true));
        
        
        GridViewUsers.DataSource = criteria.List<DatabaseEntities.User>();
        GridViewUsers.DataBind();

    }

}