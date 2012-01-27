using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Faculty_voting : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //notifications

            //voting functionality
            dbLogic.selectAllBallotPositions();
            DataSet emailSet = dbLogic.getResults();
            ListViewPositions.DataSource = emailSet;
            ListViewPositions.DataBind();

            if (dbLogic.isUserNewVoter(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
            {
                PanelSlateWrapper.Enabled = true;
                PanelSlateWrapper.Visible = true;
            }
            else
            {
                PanelSlateWrapper.Enabled = false;
                PanelSlateWrapper.Visible = false;
                LabelFeedback.Text = "You have already voted for this election.";
            }
        }
    }

    //For server transferring with hyperlinks
    protected void transfer(object sender, CommandEventArgs e)
    {
        Server.Transfer(e.CommandArgument.ToString());
    }

    //functionality for voting

    protected void ListViewPositions_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "position"))
        {
            dbLogic.selectAllForBallot(e.CommandArgument.ToString());
            DataSet emailSet = dbLogic.getResults();
            ListViewPeople.DataSource = emailSet;
            ListViewPeople.DataBind();
            PanelPeople.Visible = true;
            PanelSelect.Visible = false;
            HiddenFieldCurrentPosition.Value = e.CommandArgument.ToString();
            LabelFeedback.Text = "Select a candidate for this position.";
        }
    }

    protected void ListViewPeople_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "id"))
        {

            dbLogic.selectDetailFromWTS(e.CommandArgument.ToString());
            DataSet infoSet = dbLogic.getResults();
            DataRow dr = infoSet.Tables["query"].Rows[0];

            ButtonVote.Text = "Vote for " + dr["fullname"].ToString();
            LabelStatement.Text = "\"" + dr["statement"].ToString() + "\"";
            ButtonVote.OnClientClick = "return confirm('Are you sure you want to vote for " + dr["fullname"].ToString() + " for " + HiddenFieldCurrentPosition.Value + "?')";
            HiddenFieldName.Value = dr["fullname"].ToString();
            HiddenFieldId.Value = e.CommandArgument.ToString();

            if (LabelStatement.Text == "\"\"")
                LabelStatement.Text = dr["fullname"].ToString() + " did not include a personal statement.";

            PanelSelect.Visible = true;
            LabelFeedback.Text = "";
        }
    }

    protected void ButtonVote_Clicked(object sender, EventArgs e)
    {
        int votingCounter = 0;
        foreach (ListViewDataItem myItem in ListViewPositions.Items)
        {
            LinkButton LinkButtonPosition = (LinkButton)myItem.FindControl("LinkButtonPostions");
            Label votedFor = (Label)myItem.FindControl("LabelVoted");
            Label votedForExtra = (Label)myItem.FindControl("LabelVotedExtra");
            HiddenField votedId = (HiddenField)myItem.FindControl("HiddenFieldVotedId");
            if (LinkButtonPosition.Text == HiddenFieldCurrentPosition.Value)
            {
                votedFor.Text = HiddenFieldName.Value;
                votedForExtra.Text = "Selected ";
                votedId.Value = HiddenFieldId.Value;
                LinkButtonPosition.Enabled = false;
                PanelPeople.Visible = false;
                PanelSelect.Visible = false;
            }

            if (LinkButtonPosition.Enabled == false)
            {
                votingCounter++;
            }
        }

        int numberOfVotesLeft = ListViewPositions.Items.Count - votingCounter;

        if (votingCounter < ListViewPositions.Items.Count)
        {
            LabelFeedback.Text = "Selection for " + HiddenFieldCurrentPosition.Value + " stored. You have " + numberOfVotesLeft.ToString() + " more positions to vote on before you can submit your ballot.";
        }
        else
        {
            ButtonSubmitVotes.Visible = true;
        }
    }

    protected void ButtonSubmitVotes_Clicked(object sender, EventArgs e)
    {
        foreach (ListViewDataItem myItem in ListViewPositions.Items)
        {
            LinkButton LinkButtonPosition = (LinkButton)myItem.FindControl("LinkButtonPostions");
            HiddenField votedId = (HiddenField)myItem.FindControl("HiddenFieldVotedId"); //quick fix, should be changed later (too many hiddenfields)

            string[] votedInfo = { votedId.Value, LinkButtonPosition.Text };
            dbLogic.insertNewTally(votedInfo); //initializes tally row (if already initialized, nothing will occur)
            dbLogic.updateTally(votedInfo);
        }

        //create confirmation code
        string code = getRanString();

        //set flag that user has voted
        dbLogic.insertFlagVoted(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name), code);

        //hide slate
        PanelSlateWrapper.Enabled = false;
        PanelSlateWrapper.Visible = false;

        //give feedback
        LabelFeedback.Text = "Your ballot has been successfully submitted and processed.<br /><br /> Your confirmation code is: <b>" + code + "</b>";
    }

    protected String getRanString()
    {
        String uncleanRandomString = System.Web.Security.Membership.GeneratePassword(10, 0);
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