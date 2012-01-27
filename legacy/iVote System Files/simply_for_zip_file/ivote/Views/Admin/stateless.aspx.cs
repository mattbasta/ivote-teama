using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_stateless : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       /* if (!Page.User.Identity.IsAuthenticated)
        {
            lblForm.Visible = false;
            Server.Transfer("../../Account/Login.aspx");
        }
        if(!Page.User.IsInRole("admin"))
        {
            lblForm.Text = "You are not authorized to view this page.";
        }
        */
        
    }

    //For server transferring with hyperlinks
    protected void transfer(object sender, CommandEventArgs e)
    {
        Server.Transfer(e.CommandArgument.ToString());
    }
}