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
            Server.Transfer("/Account/Login.aspx");
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
	        bool changePasswordSuccess = logger.ChangePassword(HttpContext.Current.User.Identity.Name.ToString(), oldPW.Text, newPW.Text);
                if (changePasswordSuccess)
                {
                    lblConfirm.Text = "Your password has been successfully changed.";
                }
                else
                {
                    lblConfirm.Text = "You password change was unsuccessful. Please contact the system administrator.";
                }
                FailurePanel.Visible = false;
                SuccessPanel.Visible = true;
            }
            else
            {
                FailureMessage.Text = "Old password is incorrect.";
                FailurePanel.Visible = true;
                SuccessPanel.Visible = false;
            }

        }
    }

}
