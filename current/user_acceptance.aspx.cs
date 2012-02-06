using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class finalsite_user_acceptance : System.Web.UI.Page
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
            Response.Redirect("~/WTS.aspx/" + HiddenFieldID.Value);
        }
        else if (String.Equals(e.CommandName, "reject"))
        {
            dbLogic.userRejectedNom(dbLogic.returnUnionIDFromUsername(username), e.CommandArgument.ToString());
            
            //rejection labels
            ConfirmLabel.Text = "Thank you, your rejection  to the " + e.CommandArgument + " position has been noted.";
            ConfirmLabel.Visible = true;
            //GridViewNominations.Visible = false;
        }
    }

    protected void bindData()
    {
        dbLogic.selectAllUserNoms(dbLogic.returnUnionIDFromUsername(username));
        DataSet nominationSet = dbLogic.getResults();
        GridViewNominations.DataSource = nominationSet;
        GridViewNominations.DataBind();
    }
}