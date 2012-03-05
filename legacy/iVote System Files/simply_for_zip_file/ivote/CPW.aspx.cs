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
                string e_pw = logger.encrypt(newPW.Text);
                dblogic.updatePassword2(HttpContext.Current.User.Identity.Name.ToString(), e_pw);
                SuccessPanel.Visible = true;
                FailurePanel.Visible = false;
            }
            else
            {
                FailureMessage.Text = "Old password is incorrect.";
                FailurePanel.Visible = true;
                SuccessPanel.Visible = false;
            }
            //decrypt password fromt his
            //lblConfirm.Text = "Form in progress.";
            //change form state
            //lblConfirm.Text = "Thank you.  Your password has been changed.";
        }
    }

}
