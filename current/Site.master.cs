using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*if user is not logged in, hides menu
        if (Page.User.Identity.IsAuthenticated)
        {
            NavigationMenu.Enabled = true;
            NavigationMenu.Visible = true;
        }
        else
        {
            NavigationMenu.Enabled = false;
            NavigationMenu.Visible = false;
        }*/
    }

    //Takes a CommandArgument, using the OnCommand functionality and
    //does a server.transfer to that location
    protected void transfer(object sender, CommandEventArgs e)
    {
        Server.Transfer(e.CommandArgument.ToString());
    }
}
