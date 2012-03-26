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

public partial class phase1aSite_forgotPass : System.Web.UI.Page
{
    VerifyEmail sendEmail = new VerifyEmail();
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void submit(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();

            int userID;
            bool temp_error = false;
            string[] emailAddress = new string[1];
            emailAddress[0] = email.Text;
            String emailMessage = "";
            emailMessage += "Hello, you are receiving this email because you have forgotten your password. <br />";
            emailMessage += "Please follow the link below to reset your password. <br /><br />";
            
            // check if email exists in union_members table
            if (!DatabaseEntities.User.CheckIfEmailExists(session, email.Text))
            {
                lblError.Visible = true;
                temp_error = true;
               
            }
            else
            {
                // get userID and from email
                userID = DatabaseEntities.User.FindUser(session, email.Text).ID;

                // send email to that person if the email exists
                sendEmail.verify(userID, emailAddress, emailMessage);
            }
            
            //make form label invisible
            lblForgot.Visible = false;
            //make confirmation message label visible
            if (!temp_error)
            {
                lblConfirm.Visible = true;
            }
        }
    }
}