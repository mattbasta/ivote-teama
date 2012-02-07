using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if user is already signed in, then they cannot not view the default page (which is simply the log in page)
        if (Page.User.Identity.IsAuthenticated)
        {
            Response.Redirect("home.aspx");
        }
    }
}
