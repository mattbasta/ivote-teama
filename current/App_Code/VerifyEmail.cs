using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for VerifyEmail
/// </summary>
public class VerifyEmail
{
    databaseLogic dbLogic = new databaseLogic();
    emailer sendEmail = new emailer();

    String emailQuestions;
    ArrayList allEmails;

    public VerifyEmail()
	{
        emailQuestions = "";
        emailQuestions += "Any questions or comments please email the current APSCUF-KU iVote system administrator.<br /><br />";
        emailQuestions += "Team A<br />CSC 354<br /><br />";
        emailQuestions += "(PLEASE DO NOT REPLY TO THIS EMAIL.)";

        allEmails = new ArrayList();
	}
   
    //sends the user an email when registered
    public void verify(int unionID, string[] emailAddress, String emailMessage)
    {
        String confirmUrl = "confirm.aspx"; //current landing page for test cofirmation
        String code_confirm = getRanString(); //the confirmation code needed by user to activate the account
        String code_reject = getRanString(); //the code needed if person sent email is not person who sent verification code
        

        String emailLinks = "";
        emailLinks += "To proceed with this process, please click <a href=\"http://ivote.bashtech.net/confirm.aspx?x=" + code_confirm + "\">Here</a>.<br />";
        emailLinks += "OR, copy and paste the following URL into your address bar.<br />";
        emailLinks += "URL to verify your iVote account: http://ivote.bashtech.net/confirm.aspx?x=" + code_confirm + "<br /><br />";

        emailMessage += emailLinks;
        emailMessage += emailQuestions;
       
        //send email to new user
        sendEmail.sendEmailToList(emailAddress, emailMessage, "Welcome to the APSCUF iVote System");

        //inserting new codes for user into the db
        dbLogic.insertCodes(unionID, code_confirm, code_reject); //sends codes to inserter for db
    }


    //sends the user an email when nominated
    public void nominated(string[] emailAddress, String emailMessage)
    {
        emailMessage += emailQuestions;
        
        //send email to user
        sendEmail.sendEmailToList(emailAddress, emailMessage, "APSCUF iVote Nomination Recieved");
    }


    //------------- phase change emails ------------------//

    public void phaseNomination()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  The election has begun.";
        emailMessage += " It is now the nomination phase of the voting process.  You are able to log in and nominate yourself or another faculty member for the given positions available.  You may follow this URL to do so: <a href=\"http://ivote.bashtech.net\"> link</a> ";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;      

        string[] arrayAllEmail = dbLogic.getEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Nomination Period Started");       
    }

    //email is sent for users who have yet to accept/reject nomination
    public void phaseAccept1()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  We are sending you this because you have been nominated to run for the current election, but have yet to accept or reject the nomination.";
        emailMessage += " Please log into the site <a href=\"http://ivote.bashtech.net\"> here</a> to accept/reject the nomination. <b>If you do not choose to accept or reject this, you will be automatically dropped from the election!</b>";
        emailMessage += "<br /><br />";

        string[] arrayAllEmail = dbLogic.getNullEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Nomination, Action Required");
    }

    //emails the NEC telling them that they need to accept the slate
    public void phaseSlate()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system. The slate has been formed and awaiting your approval.";
        emailMessage += " Please follow the link to approve the slate of nominated users: <a href=\"http://ivote.bashtech.net\"> link</a> ";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        string[] arrayAllEmail = dbLogic.getNECEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Slate Created, Action Required");
    }


    public void phasePetition()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  Nominations are over and you can now view the slate.";
        emailMessage += " The petition phase of the voting process has begun.  You can submit petitions for the given positions available. You may follow this URL to do so: <a href=\"http://ivote.bashtech.net\"> link</a> ";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        string[] arrayAllEmail = dbLogic.getEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Petition Period");
    }

    //email is sent for users who have yet to accept/reject petition
    public void phaseAccept2()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  We are sending you this because you have been petitioned to run for the current election, but have yet to accept or reject this petition.";
        emailMessage += " Please log into the site <a href=\"http://ivote.bashtech.net\"> here</a> to accept/reject the petition. <b>If you do not choose to accept or reject this, you will be automatically dropped from the election!</b>";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        string[] arrayAllEmail = dbLogic.getNullEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Action Required");
    }

    //emails the admin to let them know they need to take care of the eligibility
    public void phaseApproval()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  The nominations/petitions are finalized and as an Administrator, we need you to approve or deny all applicants Willingness to Serve forms.";
        emailMessage += " This phase will last until all of the applicants' eligibility have been taken care of, so please do not forget to make these changes.";
        emailMessage += " Please follow the link to approve/deny the nominated users: <a href=\"http://ivote.bashtech.net\"> link</a> ";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        string[] arrayAllEmail = dbLogic.getAdminEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Approval Needed");
    }


    public void phaseVote()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  The Petition phase has ended.  It is now the voting phase of the election process.";
        emailMessage += " You are able to log in and vote on the candidates for each position.  You may follow this URL to do so: <a href=\"http://ivote.bashtech.net\"> link</a> ";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        string[] arrayAllEmail = dbLogic.getEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Voting Period Officially Begun");
    }

    public void phaseResults()
    {
        String emailMessage;
        emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  The election has concluded, and you are able to log in and view the results.  You may follow this URL to do so: ";
        emailMessage += "<a href=\"http://ivote.bashtech.net\"> link</a> ";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        string[] arrayAllEmail = dbLogic.getEmails();

        //send emails out
        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Election Officially Concluded");
    }


    //------------------ end phase change emails ---------------------------------//



    public void approveUserEligibility(string id)
    {
        String emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  We are informing you that your eligibility for the current election has been approved!";
        emailMessage += " You are now entered onto the slate for the upcoming voting process.";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        int num = (int)Convert.ToInt32(id);
        string[] arrayAllEmail = new string[] {dbLogic.selectEmailFromID(num)};

        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Nomination Approved");
    }


    public void denyUserEligibility(string id)
    {
        String emailMessage = "";
        emailMessage += "Hello and greetings from the APSCUF-KU iVote System! <br /><br />";
        emailMessage += "This is an automated message from the APSCUF-KU iVote system.  We are informing you that your eligibility for the current election has been denied!";
        emailMessage += " You will <b>NOT</b> be eligible for the upcoming voting process. If you are not sure why you were denied, please feel free to contact the system administrator.";
        emailMessage += "<br /><br />";
        emailMessage += emailQuestions;

        int num = (int)Convert.ToInt32(id);
        string[] arrayAllEmail = new string[] { dbLogic.selectEmailFromID(num) };

        sendEmail.sendEmailToList(arrayAllEmail, emailMessage, "APSCUF iVote Nomination Eligibility Notice");
    }




    //returns random regular expression 64 length string
    protected String getRanString()
    {
        String uncleanRandomString = System.Web.Security.Membership.GeneratePassword(62, 0);
        uncleanRandomString = uncleanRandomString.Replace("!", "a");
        uncleanRandomString = uncleanRandomString.Replace("@", "2");
        uncleanRandomString = uncleanRandomString.Replace("#", "c");
        uncleanRandomString = uncleanRandomString.Replace("$", "4");
        uncleanRandomString = uncleanRandomString.Replace("%", "3");
        uncleanRandomString = uncleanRandomString.Replace("^", "i");
        uncleanRandomString = uncleanRandomString.Replace("&", "a");
        uncleanRandomString = uncleanRandomString.Replace("*", "9");
        uncleanRandomString = uncleanRandomString.Replace("(", "g");
        uncleanRandomString = uncleanRandomString.Replace(")", "m");
        uncleanRandomString = uncleanRandomString.Replace("_", "d");
        uncleanRandomString = uncleanRandomString.Replace("-", "5");
        uncleanRandomString = uncleanRandomString.Replace("+", "p");
        uncleanRandomString = uncleanRandomString.Replace("=", "q");
        uncleanRandomString = uncleanRandomString.Replace("[", "w");
        uncleanRandomString = uncleanRandomString.Replace("{", "t");
        uncleanRandomString = uncleanRandomString.Replace("]", "r");
        uncleanRandomString = uncleanRandomString.Replace("}", "f");
        uncleanRandomString = uncleanRandomString.Replace(";", "8");
        uncleanRandomString = uncleanRandomString.Replace(":", "z");
        uncleanRandomString = uncleanRandomString.Replace("<", "x");
        uncleanRandomString = uncleanRandomString.Replace(">", "0");
        uncleanRandomString = uncleanRandomString.Replace("|", "v");
        uncleanRandomString = uncleanRandomString.Replace(".", "b");
        uncleanRandomString = uncleanRandomString.Replace("/", "y");
        uncleanRandomString = uncleanRandomString.Replace("?", "t");
        return uncleanRandomString;
    }
}