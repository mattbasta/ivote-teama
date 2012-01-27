using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_approval : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            checkForEligibility();
        }
    }

    //For server transferring with hyperlinks
    protected void transfer(object sender, CommandEventArgs e)
    {
        Server.Transfer(e.CommandArgument.ToString());
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


}