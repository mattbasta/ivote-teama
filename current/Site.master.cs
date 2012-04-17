using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.Security;

using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;


public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // On any given page (except the changing password page), check if the user's password is the password
        // which forces them to choose a new password.
        ISession session = NHibernateHelper.CreateSessionFactory().OpenSession();
        User user = User.FindUser(session, HttpContext.Current.User.Identity.Name);
        if (user != null &&
            user.Password == User.Hash(User.DefaultPassword) &&
            !Request.RawUrl.Contains("ChangePassword.aspx"))
            Response.Redirect("~/ChangePassword.aspx", false);
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
