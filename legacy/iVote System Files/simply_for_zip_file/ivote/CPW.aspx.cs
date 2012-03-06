using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.Security.Cryptography;


public partial class CPW : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //make sure user is logged in
        if (!Page.User.Identity.IsAuthenticated)
        {
            lblForm.Visible = false;
            Server.Transfer("Account/Login.aspx");
        }
    }

    protected void submission(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //check old password
            iVoteLoginProvider logger = new iVoteLoginProvider();
            databaseLogic dblogic = new databaseLogic();

            bool pw_correct = logger.ValidateUser(HttpContext.Current.User.Identity.Name.ToString(), oldPW.Text);
            if (pw_correct)
            {
                lblConfirm.Text = "Your password has been successfully changed.";
                string e_pw = logger.encrypt(newPW.Text);
                dblogic.updatePassword2(HttpContext.Current.User.Identity.Name.ToString(), e_pw);
                
            }
            else
                lblConfirm.Text = "Old password is incorrect.";
            //decrypt password fromt his
            //lblConfirm.Text = "Form in progress.";
            //change form state
            lblForm.Visible = false;
            //lblConfirm.Text = "Thank you.  Your password has been changed.";
        }
    }

}
