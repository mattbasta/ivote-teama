using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Faculty_nom_accept : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            checkForNomination();
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


}