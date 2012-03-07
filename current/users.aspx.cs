using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wwwroot_phase1aSite_users : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dbLogic.selectAllUserInfo();
            DataSet emailSet = dbLogic.getResults();
            GridViewUsers.DataSource = emailSet;
            GridViewUsers.DataBind();

            //if a user is not an admin then they cannot edit the users
            if (!User.IsInRole("admin"))
            {
                GridViewUsers.Columns[4].Visible = false;
            }
        }
    }

    protected void GridViewUsers_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "id"))
        {
            Response.Redirect("userinfo.aspx?x=" + e.CommandArgument.ToString());
            //Server.Transfer("userinfo.aspx?x=" + e.CommandArgument.ToString()); //redirects user to specific user info/edit page
        }
    }


    protected string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;

        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "DESC";
                break;
            case SortDirection.Descending:
                newSortDirection = "ASC";
                break;
        }

        return newSortDirection;
    }
    
    protected void sorting(object sender, GridViewSortEventArgs e)
    {
        string baseQuery = "SELECT * FROM union_members ORDER BY ";

        baseQuery += e.SortExpression.ToString() + " ASC"; //+ ConvertSortDirectionToSql(e.SortDirection);
        dbLogic.genericQuerySelector(baseQuery);
        DataSet emailSet = dbLogic.getResults();
        GridViewUsers.DataSource = emailSet;
        GridViewUsers.DataBind();
        
        //if a user is not an admin then they cannot edit the users
        if (!User.IsInRole("admin"))
        {
            GridViewUsers.Columns[4].Visible = false;
        }
    }

    protected void allUsers(object sender, EventArgs e)
    {
        dbLogic.selectAllUserInfo();
        DataSet emailSet = dbLogic.getResults();
        GridViewUsers.DataSource = emailSet;
        GridViewUsers.DataBind();

        //if a user is not an admin then they cannot edit the users
        if (!User.IsInRole("admin"))
        {
            GridViewUsers.Columns[4].Visible = false;
        }
        btnViewAll.Visible = false;
        txtSearch.Text = "";
    }

    protected void search(object sender, EventArgs e)
    {

        dbLogic.SelectPeopleFromSearch(txtSearch.Text);
        DataSet emailSet = dbLogic.getResults();
        GridViewUsers.DataSource = emailSet;
        GridViewUsers.DataBind();

        //if a user is not an admin then they cannot edit the users
        if (!User.IsInRole("admin"))
        {
            GridViewUsers.Columns[4].Visible = false;
        }
        if(txtSearch.Text != "")
            btnViewAll.Visible = true;

        
    }

    protected void search_adam(object sender, EventArgs e)
    {
        dbLogic.SelectPeopleFromSearch(txtSearch.Text);
        DataSet emailSet = dbLogic.getResults();
        GridViewUsers.DataSource = emailSet;
        GridViewUsers.DataBind();
        GridViewUsers.Visible = true;

        //loadConfirmPopup();
        if (!User.IsInRole("admin"))
        {
            GridViewUsers.Columns[4].Visible = false;
        }
        if (txtSearch.Text != "")
            btnViewAll.Visible = true;
    }

}