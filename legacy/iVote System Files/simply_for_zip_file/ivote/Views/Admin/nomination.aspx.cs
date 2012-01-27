using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_nomination : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();

    protected void Page_Load(object sender, EventArgs e)
    {
        Form.Action = Request.RawUrl;
        if (!Page.IsPostBack)
        {
            loadData(); //call method to load office position data
            //LabelPhase.Text = phases.currentPhase;
            checkForNomination();
            checkForEligibility();
        }
    }

    //For server transferring with hyperlinks
    protected void transfer(object sender, CommandEventArgs e)
    {
        Server.Transfer(e.CommandArgument.ToString());
    }

    /**************************************************
     * The following code is pulled from the nomination
     * related pages.
     * ************************************************/

    //load office position data
    protected void loadData()
    {
        dbLogic.selectAllAvailablePositions();
        DataSet postionsSet = dbLogic.getResults();
        GridViewPositions.DataSource = postionsSet;
        GridViewPositions.DataBind();
    }
    //gridview actions
    protected void GridViewPositions_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "positions"))
        {
            PanelSelected.Visible = true; //makes the panel visible to the user
            LabelSelected.Text = "<b>Info for " + e.CommandArgument + ":</b> <br /><br />(In the future this area will hold information about this position including<br /> what the duties are, and who's the current position holder.)"; //displays which position the user has selected
            ButtonWTS.Text = "Nominate Yourself For " + e.CommandArgument;
            ButtonNominate.Text = "Nominate A Faculty For " + e.CommandArgument;
            HiddenFieldID.Value = dbLogic.selectIDFromPosition(e.CommandArgument.ToString());
        }
    }

    //check if the user has a nomination pending
    protected void checkForNomination()
    {
        if (dbLogic.isUserNominatedPending(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
        {
            PanelNominationPending.Enabled = true;
            PanelNominationPending.Visible = true;
            nom_pending.Enabled = true;
            nom_pending.Visible = true;
        }
    }

    //check to see if there is any pending eligibility
    protected void checkForEligibility()
    {
        if (dbLogic.returnEligibilityCount() > 0)
        {
            PanelNominationPending.Enabled = true;
            PanelNominationPending.Visible = true;
            elig_pending.Enabled = true;
            elig_pending.Visible = true;
        }
    }

    protected void next(Object sender, EventArgs e)
    {
        Response.Redirect("~/WTS.aspx/" + HiddenFieldID.Value);
    }

    protected void nominate(Object sender, EventArgs e)
    {
        Response.Redirect("~/nominate.aspx/" + HiddenFieldID.Value);
    }
}