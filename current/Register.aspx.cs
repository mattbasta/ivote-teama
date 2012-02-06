using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Register : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    VerifyEmail sendEmail = new VerifyEmail();
    iVoteRoleProvider roleProvider = new iVoteRoleProvider();
    protected void Page_Load(object sender, EventArgs e)
    {
  
    }

    protected void submit(object sender, EventArgs e)
    {
        //make sure the page is valid before processing
        if (Page.IsValid)
        {
            if (dbLogic.checkIfEmailExists(Email.Text)) //checks if new user's email address already exists
            {
                LabelFeedback.Text = "Email Address/User already exists in dabase records.";
            }
            else
            {
                string[] union = { LastName.Text, FirstName.Text, Email.Text, Phone.Text, DeptDropDown.SelectedValue };
                string[] username = Email.Text.Split(new char[] { '@' }); //Take only first part of users email and make it username (ex. asd123@yahoo.com. username = asd123)
                string user = username[0];

                dbLogic.insertUnion(union);  //Insert into Union_members table
                int unionID = dbLogic.returnLastUnionAdded();

                dbLogic.insertUser(unionID, user);  //Insert into User table
             
                string[] emailAddress = new string[1];
                emailAddress[0] = Email.Text;

                //email message sent to new user
                String emailMessage = "";
                emailMessage += "Hello " + FirstName.Text + " " + LastName.Text + ",<br /><br />";
                emailMessage += "The APSCUF-KU iVote system administrator has added you as a new user!<br /><br />";
                emailMessage += "<u>You MUST verify your new account before it can be fully activated!</u> <br /><br />";
                emailMessage += "Please record your new username for the iVote System below. <br />";
                emailMessage += "(You will use this username to log onto the system.)<br /><br />";
                emailMessage += "Username: <b>" + username[0] + "</b> <br /><br />";

                // passes arguments to this class where it will send the email
                sendEmail.verify(unionID, emailAddress, emailMessage);

                //make form label invisible
                lblForm.Visible = false;
                //make confirmation message label visible
                lblConfirm.Visible = true;

                setUpRoles(user);
            }
        }
    }

    //adds role of new user
    public void setUpRoles(String user)
    {
        String[] userArray = {user};
        String[] roleArray = { RadioButtonListRoles.SelectedValue };
        roleProvider.AddUsersToRoles(userArray, roleArray);
    }
}