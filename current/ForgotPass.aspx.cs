using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;

public partial class phase1aSite_ForgotPass : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e) { }

    public void submit(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();

            int userID;

            // check if email exists in union_members table
            if (!DatabaseEntities.User.CheckIfEmailExists(session, email.Text))
            {
                lblError.Visible = true;
                return;
            }
            
            // get userID and from email
            DatabaseEntities.User user = DatabaseEntities.User.FindUser(session, email.Text);

            // send email to that person if the email exists
            nEmailHandler emailer = new nEmailHandler();
            emailer.sendConfirmationEmail(user, "APSCUF iVote System Password Reset", "userForgotPassword");
            lblConfirm.Visible = true;
            lblForgot.Visible = false;
            lblError.Visible = false;
        }
    }
}