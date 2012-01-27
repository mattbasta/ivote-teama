using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wwwroot_finalsite_terminate : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        ButtonTerminate.OnClientClick = "return confirm('Are you sure you want to offically terminate this election?')";
    }

    protected void ButtonTerminate_clicked(object sender, EventArgs e)
    {
        //reset system
        dbLogic.resetElection();

        string newMessage = "Hello all users of the iVote System,<br /><br />This email is to inform you that the current administrator has offically <u>terminated</u> the current election.<br /><br/>";
        newMessage += "<b>The administrator has included this message explaining why the current election has been terminated:</b><br /><br />";
        newMessage += "\"" + TextBoxMessage.Text + "\"<br /><br />";
        newMessage += "If you have any questions, comments, or concerns involving this development please email the current administrator of the iVote system.<br /><br /><br />The iVote Team";

        string[] emailList;

        emailer emailSender = new emailer();

        //grab full list of emails
        emailList = dbLogic.getEmails();

        emailSender.sendEmailToList(emailList, newMessage, "Current Election Offically Terminated");


        //give feedback to the user
        PanelTerminate.Visible = false;
        PanelTerminate.Enabled = false;

        PanelComplete.Visible = true;
        PanelComplete.Enabled = true;
    }
}