using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class phase1aSite_forgotPass : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    VerifyEmail sendEmail = new VerifyEmail();
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void submit(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int unionID;
            bool temp_error = false;
            string[] emailAddress = new string[1];
            emailAddress[0] = email.Text;
            String emailMessage = "";
            emailMessage += "Hello, you are receiving this email because you have forgotten your password. <br />";
            emailMessage += "Please follow the link below to reset your password. <br /><br />";
            
            // check if email exists in union_members table
            if (!dbLogic.checkIfEmailExists(email.Text))
            {
                lblError.Visible = true;
                temp_error = true;
               
            }
            else
            {
                // get unionID and from email
                unionID = dbLogic.selectIDFromEmail(email.Text);

                // send email to that person if the email exists
                sendEmail.verify(unionID, emailAddress, emailMessage);
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