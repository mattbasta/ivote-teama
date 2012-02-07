using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wwwroot_finalsite_Nominations : System.Web.UI.Page
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

    protected void GridViewNominations_ItemCommand(object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "accept"))
        {
            if (dbLogic.isUserWTS(dbLogic.returnUnionIDFromUsername(username), e.CommandArgument.ToString()))
            {
                //change accepted in nomination_accept table to 1
                dbLogic.userAcceptedNom( dbLogic.returnUnionIDFromUsername(username), e.CommandArgument.ToString());

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
            dbLogic.userRejectedNom( dbLogic.returnUnionIDFromUsername(username), e.CommandArgument.ToString() );
            ConfirmLabel.Text = "Your rejection to the " + e.CommandArgument + " position has been acknowleged.";
            ConfirmLabel.Visible = true;
            bindData();
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