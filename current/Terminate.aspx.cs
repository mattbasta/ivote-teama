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

        string newMessage = "Hello,<br /><br />This email is to notify you that the current APSCUF election administration has offically <u>terminated</u> the current officer election.<br /><br/>";
        newMessage += "<p><b>The administrator has included this message explaining why the current election has been terminated:</b></p>";
        newMessage += "<blockquote>" + Server.HtmlEncode(TextBoxMessage.Text) + "</blockquote>";
        newMessage += "<p>If you have any questions, comments, or concerns involving this development please email the current administrator of the iVote system.</p><p>-Kutztown APSCUF</p>";


        //grab full list of emails
        string[] emailList;
        emailList = dbLogic.getEmails();

        emailer emailSender = new emailer();
        emailSender.sendEmailToList(emailList, newMessage, "APSCUF Officer Election Terminated");

        //give feedback to the user
        PanelTerminate.Visible = false;
        PanelTerminate.Enabled = false;

        PanelComplete.Visible = true;
        PanelComplete.Enabled = true;
    }
}